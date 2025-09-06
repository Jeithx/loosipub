using Api.Helper.Middleware;
using API;
using API.Jobs;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Business.AutoMapperProfiles;
using Business.Concrete;
using Core.DependencyResolvers;
using Core.Extensions;
using Core.Utilities.IoC;
using Core.Utilities.Security.Encyption;
using Core.Utilities.Security.Jwt;
using Core.Utilities.Security.RateLimiting;
using Core.Utilities.Security.RateLimiting.Models;
using Core.Utilities.Security.RateLimiting.Services;
using Core.Utilities.Security.RequestTracking;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true);

#region RateLimiting&RequestTracking
// Serilog konfigürasyonu
Serilog.Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.HttpLogging", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithThreadId()
    .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File("logs/api-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.File("logs/security-.log",
        restrictedToMinimumLevel: LogEventLevel.Warning,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 90,
        outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [SECURITY] [{Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestHeaders |
                            HttpLoggingFields.RequestBody |
                            HttpLoggingFields.ResponseHeaders |
                            HttpLoggingFields.ResponseBody |
                            HttpLoggingFields.RequestQuery;

    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;

    // Hassas header'larý gizle
    options.RequestHeaders.Add("Authorization");
    options.RequestHeaders.Add("X-API-Key");
});

// Rate Limiting konfigürasyonu
var rateLimitOptions = new RateLimitOptions();
builder.Configuration.GetSection("RateLimit").Bind(rateLimitOptions);
builder.Services.AddSingleton(rateLimitOptions);
// Rate Limiting service'ini ekle
builder.Services.AddSingleton<IRateLimitingService, InMemoryRateLimitingService>();
// Custom middleware
builder.Services.AddScoped<RequestTrackingMiddleware>();
builder.Services.AddScoped<RateLimitingMiddleware>();
#endregion

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddDistributedMemoryCache();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureContainer<ContainerBuilder>(b =>
{
    b.RegisterModule(new Business.DependencyResolvers.Autofac.AutofacBusinessModule());
});

builder.Services.AddAutoMapper(typeof(Profiles));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


builder.Services.AddSwaggerGen(c =>
{
    c.CustomSchemaIds(type => type.FullName);
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });

    //// API Key zorunlu
    //c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    //{
    //    Name = "X-API-KEY",
    //    Type = SecuritySchemeType.ApiKey,
    //    In = ParameterLocation.Header,
    //    Description = "API Key'inizi girin"
    //});

    // JWT Token (opsiyonel)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token (Bazý API'ler için gerekli)"
    });

    // API Key'i tüm isteklere zorunlu yap
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new List<string>() // API Key tüm isteklere zorunlu
        }
    });

    // Bearer Token'ý zorunlu yapmadan tanýmlýyoruz
});

builder.Services.AddEndpointsApiExplorer();

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddSignalR(s =>
{
    s.MaximumReceiveMessageSize = long.MaxValue;
    s.EnableDetailedErrors = true;
});
builder.Services.AddDependencyResolvers(new ICoreModule[]
{
    new CoreModule()
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = tokenOptions?.Issuer,
        ValidAudience = tokenOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
        ClockSkew = TimeSpan.Zero
    };
});

// db
var connectionString = builder.Configuration.GetConnectionString("AppDbContextConnection") ??
                       throw new InvalidOperationException("Connection string 'AppDbContextConnection' not found.");
builder.Services.AddSingleton<LoosipDbContext>();

builder.Services.AddQuartz(q =>
{
    // Job'larý ekleyelim
    q.UseMicrosoftDependencyInjectionJobFactory();

    var storyJobKey = new JobKey("StoryJob");
    var userUnlockedJobKey = new JobKey("UserUnlockedJob");

    q.AddJob<StoryJob>(opts => opts.WithIdentity(storyJobKey));
    q.AddJob<UserUnlockedJob>(opts => opts.WithIdentity(userUnlockedJobKey));

    //Saatte bir kez çalýþacak þekilde ayarlandý
    q.AddTrigger(opts => opts
        .ForJob(storyJobKey)
        .StartNow()
        .WithIdentity("StoryJob-trigger")
        .WithSimpleSchedule(x => x
            .WithIntervalInHours(1)
            .RepeatForever()));

    // Her gece 00:01'de çalýþacak þekilde ayarlandý
    q.AddTrigger(opts => opts
        .ForJob(userUnlockedJobKey)
        .WithIdentity("UserUnlockedJob-trigger")
        .WithCronSchedule("1 0 0 * * ?")); // Her gece 00:01'de çalýþýr
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


var serviceProvider = builder.Services.BuildServiceProvider();
builder.Services.AddTransient<Business.Abstract.ILogger, Logger>();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = null; // 50 * 1024 * 1024
});


builder.Services.AddSignalR();

var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Test"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region RateLimiting&RequestTracking
app.UseMiddleware<RateLimitingMiddleware>();

app.UseSerilogRequestLogging(options =>
{
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
        diagnosticContext.Set("ClientIP", GetClientIP(httpContext));
        diagnosticContext.Set("ApiKey", httpContext.Request.Headers["X-API-Key"].FirstOrDefault()?.Substring(0, 8) + "...");

        if (httpContext.User.Identity?.IsAuthenticated == true)
        {
            diagnosticContext.Set("UserId", httpContext.User.FindFirst("sub")?.Value ?? httpContext.User.FindFirst("id")?.Value);
            diagnosticContext.Set("UserName", httpContext.User.Identity.Name);
        }
    };
});
app.UseMiddleware<RequestTrackingMiddleware>();
#endregion

app.UseCors("AllowAllOrigins");
//app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
//app.UseMiddleware<ResponseLoggingMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapHub<SignalHub>("/signalhub");
app.MapControllers(); // API controller'larýný ekler.

try
{
    Serilog.Log.Information("API baþlatýlýyor...");
    app.Run();
}
catch (Exception ex)
{
    Serilog.Log.Fatal(ex, "API baþlatýlamadý!");
}
finally
{
    Serilog.Log.CloseAndFlush();
}

static string GetClientIP(HttpContext context)
{
    var xForwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
    if (!string.IsNullOrEmpty(xForwardedFor))
    {
        return xForwardedFor.Split(',')[0].Trim();
    }

    var xRealIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
    if (!string.IsNullOrEmpty(xRealIp))
    {
        return xRealIp;
    }

    return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
}
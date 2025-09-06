using Microsoft.Extensions.Logging;
using ILogger = Business.Abstract.ILogger;

namespace Business.Concrete;

public class Logger : ILogger
{
    //private readonly Lazy<IMailLogService> _mailLogService;
    //private readonly MailHelperService _mailHelperService;
    //private readonly Lazy<ILogService> _logService;
    private readonly ILogger<Logger> _logger;

    public Logger(ILogger<Logger> logger)
    {
        _logger = logger;
    }



    //public Logger(Lazy<IMailLogService> mailLogService, MailHelperService mailHelperService,
    //    Lazy<ILogService> logService, ILogger<Logger> logger)
    //{
    //    _mailLogService = mailLogService;
    //    _mailHelperService = mailHelperService;
    //    _logService = logService;
    //    _logger = logger;
    //}

    public void LogException(Exception exception, string methodName, object? parameters = null)
    {
        _logger.LogError(exception, "An error occurred in {MethodName}. Parameters: {@Parameters}", methodName, parameters);

        var message = $"An error occurred in {methodName}\n\n" +
                      $"Exception: {exception.Message}\n" +
                      $"Inner Exception: {exception.InnerException}\n" +
                      $"StackTrace: {exception.StackTrace}\n" +
                      $"Parameters: {System.Text.Json.JsonSerializer.Serialize(parameters)}";

        //_logService.Value.Create(new LogWD
        //{
        //    LogType = ELogType.Exception,
        //    RequestUrl = "",
        //    HttpMethod = methodName,
        //    RequestHeaders = "",
        //    RequestBody = System.Text.Json.JsonSerializer.Serialize(parameters ?? ""),
        //    StatusCode = 500,
        //    ExceptionMessage = exception.InnerException?.ToString() ?? "",
        //    CreateTime = DateTime.Now,
        //    UpdateTime = DateTime.Now,
        //    IA = true
        //});

        //SendLogMail(message);
    }

    public void LogInformation(string methodName, object? parameters = null)
    {
        _logger.LogInformation("{MethodName} executed. Parameters: {@Parameters}", methodName, parameters);
    }

    private void SendLogMail(string message)
    {
        try
        {
            //var mailResponse =
                //_mailHelperService.SendMail(message, "Critical Exception Occurred", "mrtcnrdmii@gmail.com");

            // Update log status to success
            //_mailLogService.Value.Create(new MailLogWD
            //{
            //    Subject = "Critical Exception Occurred",
            //    Explanation = message,
            //    Sender = "noreply@trendpiyasa.com",
            //    Receiver = "mrtcnrdmii@gmail.com",
            //    SendTime = DateTime.Now,
            //    IsSuccess = mailResponse.Result
            //});
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send exception email.");
        }
    }

    public void LogNotFound<T>(long id, string methodName)
    {
        _logger.LogWarning("{Entity} not found in {Method}. ID: {Id}", typeof(T).Name, methodName, id);
    }
}
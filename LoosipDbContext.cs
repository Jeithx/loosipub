using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Entities.Models;

public partial class LoosipDbContext : DbContext
{
    public LoosipDbContext()
    {
    }

    public LoosipDbContext(DbContextOptions<LoosipDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BlockedUser> BlockedUsers { get; set; }

    public virtual DbSet<Call> Calls { get; set; }

    public virtual DbSet<CallPayment> CallPayments { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Faq> Faqs { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostComment> PostComments { get; set; }

    public virtual DbSet<PostLike> PostLikes { get; set; }

    public virtual DbSet<PostMedia> PostMedias { get; set; }

    public virtual DbSet<PostTag> PostTags { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<SearchLog> SearchLogs { get; set; }

    public virtual DbSet<SocialMedia> SocialMedias { get; set; }

    public virtual DbSet<Subscriber> Subscribers { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Tip> Tips { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserActivityLog> UserActivityLogs { get; set; }

    public virtual DbSet<UserBan> UserBans { get; set; }

    public virtual DbSet<UserCallPrice> UserCallPrices { get; set; }

    public virtual DbSet<UserDocument> UserDocuments { get; set; }

    public virtual DbSet<UserFollower> UserFollowers { get; set; }

    public virtual DbSet<UserLoginAttempt> UserLoginAttempts { get; set; }

    public virtual DbSet<UserSetting> UserSettings { get; set; }

    public virtual DbSet<UserSocialMedia> UserSocialMedias { get; set; }

    public virtual DbSet<UserSubscription> UserSubscriptions { get; set; }

    public virtual DbSet<UserSubscriptionPlan> UserSubscriptionPlans { get; set; }

    public virtual DbSet<UserWarning> UserWarnings { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<WalletLog> WalletLogs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .Build();
            var connectionString = configuration.GetConnectionString("AppDbContextConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<BlockedUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("BlockedUsers_PK");

            entity.HasIndex(e => e.BlockedUserId, "IX_BlockedUsers_BlockedUserId");

            entity.HasIndex(e => e.UserId, "IX_BlockedUsers_UserId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.BlockedUserNavigation).WithMany(p => p.BlockedUserBlockedUserNavigations)
                .HasForeignKey(d => d.BlockedUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BlockedUsers_Users_FK_1");

            entity.HasOne(d => d.User).WithMany(p => p.BlockedUserUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("BlockedUsers_Users_FK");
        });

        modelBuilder.Entity<Call>(entity =>
        {
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.CallerUser).WithMany(p => p.CallCallerUsers)
                .HasForeignKey(d => d.CallerUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Calls_Users1");

            entity.HasOne(d => d.ReceiverUser).WithMany(p => p.CallReceiverUsers)
                .HasForeignKey(d => d.ReceiverUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Calls_Users");
        });

        modelBuilder.Entity<CallPayment>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaidDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Call).WithMany(p => p.CallPayments)
                .HasForeignKey(d => d.CallId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CallPayments_Calls");

            entity.HasOne(d => d.Payer).WithMany(p => p.CallPayments)
                .HasForeignKey(d => d.PayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CallPayments_Users");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Cities_PK");

            entity.HasIndex(e => e.CountryId, "IX_Cities_CountryId");

            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Cities_Countries_FK");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Countries_PK");

            entity.HasIndex(e => e.LanguageId, "IX_Countries_LanguageId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Language).WithMany(p => p.Countries)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Countries_Languages_FK");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Documents_PK");

            entity.HasIndex(e => e.LanguageId, "IX_Documents_LanguageId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FileUrl)
                .UseCollation("Turkish_CI_AS")
                .HasColumnName("FileURL");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Language).WithMany(p => p.Documents)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Documents_Languages_FK");
        });

        modelBuilder.Entity<Faq>(entity =>
        {
            entity.HasIndex(e => e.LanguageId, "IX_Faqs_LanguageId");

            entity.Property(e => e.Answer).UseCollation("Turkish_CI_AS");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Question)
                .HasMaxLength(500)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Language).WithMany(p => p.Faqs)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Faqs_Languages");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Genders_PK");

            entity.HasIndex(e => e.LanguageId, "IX_Genders_LanguageId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Language).WithMany(p => p.Genders)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Genders_Languages_FK");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Languages_PK");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.LangCode)
                .HasMaxLength(20)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Logs_PK");

            entity.Property(e => e.Action)
                .HasMaxLength(100)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.Exception).UseCollation("Turkish_CI_AS");
            entity.Property(e => e.LogDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Service)
                .HasMaxLength(100)
                .UseCollation("Turkish_CI_AS");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Messages_PK");

            entity.HasIndex(e => e.ReceiverId, "IX_Messages_ReceiverId");

            entity.HasIndex(e => e.SenderId, "IX_Messages_SenderId");

            entity.Property(e => e.FileUrl).UseCollation("Turkish_CI_AS");
            entity.Property(e => e.Message1)
                .HasMaxLength(500)
                .UseCollation("Turkish_CI_AS")
                .HasColumnName("Message");
            entity.Property(e => e.SendDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Messages_Users_FK_1");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Messages_Users_FK");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Notifications_PK");

            entity.HasIndex(e => e.UserId, "IX_Notifications_UserId");

            entity.Property(e => e.Content)
                .HasMaxLength(1000)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Notifications_Users_FK");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Posts_PK");

            entity.HasIndex(e => e.UserId, "IX_Posts_UserId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsNsfw).HasColumnName("IsNSFW");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ScheduledFor).HasColumnType("datetime");
            entity.Property(e => e.Title)
                .HasMaxLength(1000)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Posts_Users_FK");
        });

        modelBuilder.Entity<PostComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PostComments_PK");

            entity.HasIndex(e => e.ParentId, "IX_PostComments_ParentId");

            entity.HasIndex(e => e.PostId, "IX_PostComments_PostId");

            entity.HasIndex(e => e.UserId, "IX_PostComments_UserId");

            entity.Property(e => e.Comments)
                .HasMaxLength(300)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MediaUrl).UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("PostComments_PostComments_FK");

            entity.HasOne(d => d.Post).WithMany(p => p.PostComments)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostCommentsa_Posts_FK");

            entity.HasOne(d => d.User).WithMany(p => p.PostComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostCommentsa_Users_FK");
        });

        modelBuilder.Entity<PostLike>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.PostId });

            entity.HasIndex(e => e.PostId, "IX_PostLikes_PostId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.PostLikes)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostLikes_Posts_FK");

            entity.HasOne(d => d.User).WithMany(p => p.PostLikes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostLikes_Users_FK");
        });

        modelBuilder.Entity<PostMedia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PostMedias_PK");

            entity.HasIndex(e => e.PostId, "IX_PostMedias_PostId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MediaUrl)
                .UseCollation("Turkish_CI_AS")
                .HasColumnName("MediaURL");
            entity.Property(e => e.Order).HasDefaultValue(1);
            entity.Property(e => e.ThumbnailUrl)
                .UseCollation("Turkish_CI_AS")
                .HasColumnName("ThumbnailURL");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.PostMedia)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostMedias_Posts_FK");
        });

        modelBuilder.Entity<PostTag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PostTags_PK");

            entity.HasIndex(e => e.PostId, "IX_PostTags_PostId");

            entity.HasIndex(e => e.TagId, "IX_PostTags_TagId");

            entity.HasOne(d => d.Post).WithMany(p => p.PostTags)
                .HasForeignKey(d => d.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostTags_Posts_FK");

            entity.HasOne(d => d.Tag).WithMany(p => p.PostTags)
                .HasForeignKey(d => d.TagId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PostTags_Tags_FK");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Reports_PK");

            entity.HasIndex(e => e.PostId, "IX_Reports_PostId");

            entity.HasIndex(e => e.ReportedUserId, "IX_Reports_ReportedUserId");

            entity.HasIndex(e => e.ReporterId, "IX_Reports_ReporterId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Reason)
                .HasMaxLength(1000)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Post).WithMany(p => p.Reports)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("Reports_Posts_FK");

            entity.HasOne(d => d.ReportedUser).WithMany(p => p.ReportReportedUsers)
                .HasForeignKey(d => d.ReportedUserId)
                .HasConstraintName("Reports_Users_FK_1");

            entity.HasOne(d => d.Reporter).WithMany(p => p.ReportReporters)
                .HasForeignKey(d => d.ReporterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Reports_Users_FK");
        });

        modelBuilder.Entity<SearchLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SearchLogs_PK");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.SearchKey)
                .HasMaxLength(150)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<SocialMedia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SocialMedias_PK");

            entity.HasIndex(e => e.LanguageId, "IX_SocialMedias_LanguageId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Logo)
                .HasMaxLength(100)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Language).WithMany(p => p.SocialMedia)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SocialMedias_Languages_FK");
        });

        modelBuilder.Entity<Subscriber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Subscribers_PK");

            entity.HasIndex(e => e.SubscriberId, "IX_Subscribers_SubscriberId");

            entity.HasIndex(e => e.UserId, "IX_Subscribers_UserId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.SubscriberNavigation).WithMany(p => p.SubscriberSubscriberNavigations)
                .HasForeignKey(d => d.SubscriberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Subscribers_Users_FK_1");

            entity.HasOne(d => d.User).WithMany(p => p.SubscriberUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Subscribers_Users_FK");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tags_PK");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Tip>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tips_PK");

            entity.HasIndex(e => e.SenderId, "IX_Tips_SenderId");

            entity.HasIndex(e => e.UserId, "IX_Tips_UserId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Sender).WithMany(p => p.TipSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tips_Users_FK_1");

            entity.HasOne(d => d.User).WithMany(p => p.TipUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Tips_Users_FK");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_PK");

            entity.HasIndex(e => e.CityId, "IX_Users_CityId");

            entity.HasIndex(e => e.CountryId, "IX_Users_CountryId");

            entity.HasIndex(e => e.GenderId, "IX_Users_GenderId");

            entity.HasIndex(e => e.PrefferedLanguageId, "IX_Users_PrefferedLanguageId");

            entity.Property(e => e.Biography).HasMaxLength(1000);
            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.CoverPictureUrl).UseCollation("Turkish_CI_AS");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DisplayName)
                .HasMaxLength(100)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.LastLoginAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.ProfilePictureUrl).UseCollation("Turkish_CI_AS");
            entity.Property(e => e.RefferalCode)
                .HasMaxLength(10)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .UseCollation("Turkish_CI_AS");

            entity.HasOne(d => d.City).WithMany(p => p.Users)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Cities_FK");

            entity.HasOne(d => d.Country).WithMany(p => p.Users)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Countries_FK");

            entity.HasOne(d => d.Gender).WithMany(p => p.Users)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Genders_FK");

            entity.HasOne(d => d.PrefferedLanguage).WithMany(p => p.Users)
                .HasForeignKey(d => d.PrefferedLanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Users_Languages_FK");
        });

        modelBuilder.Entity<UserActivityLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserActivityLogs_PK");

            entity.HasIndex(e => e.ContentCreatorId, "IX_UserActivityLogs_ContentCreatorId");

            entity.HasIndex(e => e.PostId, "IX_UserActivityLogs_PostId");

            entity.HasIndex(e => e.TagId, "IX_UserActivityLogs_TagId");

            entity.HasIndex(e => e.UserId, "IX_UserActivityLogs_UserId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.ContentCreator).WithMany(p => p.UserActivityLogContentCreators)
                .HasForeignKey(d => d.ContentCreatorId)
                .HasConstraintName("UserActivityLogs_Users_FK_1");

            entity.HasOne(d => d.Post).WithMany(p => p.UserActivityLogs)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("UserActivityLogs_Posts_FK");

            entity.HasOne(d => d.Tag).WithMany(p => p.UserActivityLogs)
                .HasForeignKey(d => d.TagId)
                .HasConstraintName("UserActivityLogs_Tags_FK");

            entity.HasOne(d => d.User).WithMany(p => p.UserActivityLogUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserActivityLogs_Users_FK");
        });

        modelBuilder.Entity<UserBan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserBans_PK");

            entity.HasIndex(e => e.ModeratorId, "IX_UserBans_ModeratorId");

            entity.HasIndex(e => e.UserId, "IX_UserBans_UserId");

            entity.Property(e => e.BanDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Reason)
                .HasMaxLength(500)
                .UseCollation("Turkish_CI_AS");

            entity.HasOne(d => d.Moderator).WithMany(p => p.UserBanModerators)
                .HasForeignKey(d => d.ModeratorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserBans_Users_FK_1");

            entity.HasOne(d => d.User).WithMany(p => p.UserBanUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserBans_Users_FK");
        });

        modelBuilder.Entity<UserCallPrice>(entity =>
        {
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RatePerMinuute).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserCallPrices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserCallPrices_Users");
        });

        modelBuilder.Entity<UserDocument>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserDocuments_PK");

            entity.HasIndex(e => e.DocumentId, "IX_UserDocuments_DocumentId");

            entity.HasIndex(e => e.UserId, "IX_UserDocuments_UserId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Document).WithMany(p => p.UserDocuments)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserDocuments_Documents_FK");

            entity.HasOne(d => d.User).WithMany(p => p.UserDocuments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserDocuments_Users_FK");
        });

        modelBuilder.Entity<UserFollower>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserFollowers_PK");

            entity.HasIndex(e => e.FollowedId, "IX_UserFollowers_FollowerId");

            entity.HasIndex(e => e.FollowerId, "IX_UserFollowers_UserId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Followed).WithMany(p => p.UserFollowerFolloweds)
                .HasForeignKey(d => d.FollowedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserFollowers_Users_FK_1");

            entity.HasOne(d => d.Follower).WithMany(p => p.UserFollowerFollowers)
                .HasForeignKey(d => d.FollowerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserFollowers_Users_FK");
        });

        modelBuilder.Entity<UserLoginAttempt>(entity =>
        {
            entity.Property(e => e.AttemptDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(50)
                .HasColumnName("IPAddress");
            entity.Property(e => e.UserAgent).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.UserLoginAttempts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserLoginAttempts_Users");
        });

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("UserSettings_PK");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.AllowCommentsOnPosts).HasDefaultValue(true);
            entity.Property(e => e.BlurNsfwcontent)
                .HasDefaultValue(true)
                .HasColumnName("BlurNSFWContent");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EnableDmsFromEveryone)
                .HasDefaultValue(true)
                .HasColumnName("EnableDMsFromEveryone");
            entity.Property(e => e.IsNsfwcreator).HasColumnName("IsNSFWCreator");
            entity.Property(e => e.ReceiveEmailNotifications).HasDefaultValue(true);
            entity.Property(e => e.ReceivePushNotifications).HasDefaultValue(true);
            entity.Property(e => e.ShowFollowerCount).HasDefaultValue(true);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithOne(p => p.UserSetting)
                .HasForeignKey<UserSetting>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserSettings_Users_FK");
        });

        modelBuilder.Entity<UserSocialMedia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserSocialMedias_PK");

            entity.HasIndex(e => e.SocialMediaId, "IX_UserSocialMedias_SocialMediaId");

            entity.HasIndex(e => e.UserId, "IX_UserSocialMedias_UserId");

            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.Url)
                .HasMaxLength(150)
                .UseCollation("Turkish_CI_AS")
                .HasColumnName("URL");

            entity.HasOne(d => d.SocialMedia).WithMany(p => p.UserSocialMedia)
                .HasForeignKey(d => d.SocialMediaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserSocialMedias_SocialMedias_FK");

            entity.HasOne(d => d.User).WithMany(p => p.UserSocialMedia)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSocialMedias_Users");
        });

        modelBuilder.Entity<UserSubscription>(entity =>
        {
            entity.Property(e => e.CreaitonDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.StartDate).HasColumnType("datetime");

            entity.HasOne(d => d.SubscriptionPlan).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.SubscriptionPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSubscriptions_UserSubscriptionPlans");

            entity.HasOne(d => d.User).WithMany(p => p.UserSubscriptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSubscriptions_Users");
        });

        modelBuilder.Entity<UserSubscriptionPlan>(entity =>
        {
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrencyCode)
                .HasMaxLength(3)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.ContentCreatorUser).WithMany(p => p.UserSubscriptionPlans)
                .HasForeignKey(d => d.ContentCreatorUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserSubscriptionPlans_Users");
        });

        modelBuilder.Entity<UserWarning>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserWarnings_PK");

            entity.HasIndex(e => e.ModeratorId, "IX_UserWarnings_ModeratorId");

            entity.HasIndex(e => e.RelatedReportId, "IX_UserWarnings_RelatedReportId");

            entity.HasIndex(e => e.UserId, "IX_UserWarnings_UserId");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Reason)
                .HasMaxLength(500)
                .UseCollation("Turkish_CI_AS");
            entity.Property(e => e.WarningDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Moderator).WithMany(p => p.UserWarningModerators)
                .HasForeignKey(d => d.ModeratorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserWarnings_Users_FK_1");

            entity.HasOne(d => d.RelatedReport).WithMany(p => p.UserWarnings)
                .HasForeignKey(d => d.RelatedReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserWarnings_Reports_FK");

            entity.HasOne(d => d.User).WithMany(p => p.UserWarningUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserWarnings_Users_FK");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Wallets_PK");

            entity.HasIndex(e => e.UserId, "IX_Wallets_UserId");

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Wallets_Users_FK");
        });

        modelBuilder.Entity<WalletLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("WalletLogs_PK");

            entity.HasIndex(e => e.WalletId, "IX_WalletLogs_WalletId");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Wallet).WithMany(p => p.WalletLogs)
                .HasForeignKey(d => d.WalletId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("WalletLogs_Wallets_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

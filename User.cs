using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class User
{
    public long Id { get; set; }

    public int GenderId { get; set; }

    public string? DisplayName { get; set; }

    public string UserName { get; set; } = null!;

    public string? Biography { get; set; }

    public string Email { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public byte[] PasswordHash { get; set; } = null!;

    public byte[] PasswordSalt { get; set; } = null!;

    public string? ProfilePictureUrl { get; set; }

    public string? CoverPictureUrl { get; set; }

    public DateTime? BirthDate { get; set; }

    public int UserTypeId { get; set; }

    public bool? IsVerified { get; set; }

    public int Status { get; set; }

    public string? RefferalCode { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public DateTime LastLoginAt { get; set; }

    public bool IsActive { get; set; }

    public long CountryId { get; set; }

    public long CityId { get; set; }

    public int PrefferedLanguageId { get; set; }

    public bool IsLocked { get; set; }

    public virtual ICollection<BlockedUser> BlockedUserBlockedUserNavigations { get; set; } = new List<BlockedUser>();

    public virtual ICollection<BlockedUser> BlockedUserUsers { get; set; } = new List<BlockedUser>();

    public virtual ICollection<Call> CallCallerUsers { get; set; } = new List<Call>();

    public virtual ICollection<CallPayment> CallPayments { get; set; } = new List<CallPayment>();

    public virtual ICollection<Call> CallReceiverUsers { get; set; } = new List<Call>();

    public virtual City City { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual Gender Gender { get; set; } = null!;

    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();

    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Language PrefferedLanguage { get; set; } = null!;

    public virtual ICollection<Report> ReportReportedUsers { get; set; } = new List<Report>();

    public virtual ICollection<Report> ReportReporters { get; set; } = new List<Report>();

    public virtual ICollection<Subscriber> SubscriberSubscriberNavigations { get; set; } = new List<Subscriber>();

    public virtual ICollection<Subscriber> SubscriberUsers { get; set; } = new List<Subscriber>();

    public virtual ICollection<Tip> TipSenders { get; set; } = new List<Tip>();

    public virtual ICollection<Tip> TipUsers { get; set; } = new List<Tip>();

    public virtual ICollection<UserActivityLog> UserActivityLogContentCreators { get; set; } = new List<UserActivityLog>();

    public virtual ICollection<UserActivityLog> UserActivityLogUsers { get; set; } = new List<UserActivityLog>();

    public virtual ICollection<UserBan> UserBanModerators { get; set; } = new List<UserBan>();

    public virtual ICollection<UserBan> UserBanUsers { get; set; } = new List<UserBan>();

    public virtual ICollection<UserCallPrice> UserCallPrices { get; set; } = new List<UserCallPrice>();

    public virtual ICollection<UserDocument> UserDocuments { get; set; } = new List<UserDocument>();

    public virtual ICollection<UserFollower> UserFollowerFolloweds { get; set; } = new List<UserFollower>();

    public virtual ICollection<UserFollower> UserFollowerFollowers { get; set; } = new List<UserFollower>();

    public virtual ICollection<UserLoginAttempt> UserLoginAttempts { get; set; } = new List<UserLoginAttempt>();

    public virtual UserSetting? UserSetting { get; set; }

    public virtual ICollection<UserSocialMedia> UserSocialMedia { get; set; } = new List<UserSocialMedia>();

    public virtual ICollection<UserSubscriptionPlan> UserSubscriptionPlans { get; set; } = new List<UserSubscriptionPlan>();

    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();

    public virtual ICollection<UserWarning> UserWarningModerators { get; set; } = new List<UserWarning>();

    public virtual ICollection<UserWarning> UserWarningUsers { get; set; } = new List<UserWarning>();

    public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
}

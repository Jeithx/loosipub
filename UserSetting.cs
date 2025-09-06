using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserSetting
{
    public long UserId { get; set; }

    public bool IsSubscriptionEnabled { get; set; }

    public bool ShowFollowerCount { get; set; }

    public bool AllowCommentsOnPosts { get; set; }

    public bool ReceiveEmailNotifications { get; set; }

    public bool ReceivePushNotifications { get; set; }

    public bool EnableDmsFromEveryone { get; set; }

    public bool BlurNsfwcontent { get; set; }

    public bool IsNsfwcreator { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual User User { get; set; } = null!;
}

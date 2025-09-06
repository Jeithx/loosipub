using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserBan
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long ModeratorId { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime BanDate { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public bool IsPermanent { get; set; }

    public bool IsActive { get; set; }

    public virtual User Moderator { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class BlockedUser
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long BlockedUserId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual User BlockedUserNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

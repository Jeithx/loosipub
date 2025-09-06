using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Notification
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int Type { get; set; }

    public string Content { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class PostLike
{
    public long UserId { get; set; }

    public long PostId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

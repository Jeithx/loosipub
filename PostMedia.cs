using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class PostMedia
{
    public long Id { get; set; }

    public long PostId { get; set; }

    public string MediaUrl { get; set; } = null!;

    public string? ThumbnailUrl { get; set; }

    public string? BlurredUrl { get; set; }

    public int Type { get; set; }

    public int Order { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Post Post { get; set; } = null!;
}

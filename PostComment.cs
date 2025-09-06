using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class PostComment
{
    public long Id { get; set; }

    public long PostId { get; set; }

    public long UserId { get; set; }

    public string Comments { get; set; } = null!;

    public string? MediaUrl { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public long? ParentId { get; set; }

    public virtual ICollection<PostComment> InverseParent { get; set; } = new List<PostComment>();

    public virtual PostComment? Parent { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

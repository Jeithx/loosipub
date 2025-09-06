using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Tag
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

    public virtual ICollection<UserActivityLog> UserActivityLogs { get; set; } = new List<UserActivityLog>();
}

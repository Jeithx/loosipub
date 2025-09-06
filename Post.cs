using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Post
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public long UserId { get; set; }

    public int MediaType { get; set; }

    public int Visibility { get; set; }

    public decimal? Price { get; set; }

    public bool IsPinned { get; set; }

    public bool IsScheduled { get; set; }

    public DateTime? ScheduledFor { get; set; }

    public bool IsNsfw { get; set; }

    public int LikeCount { get; set; }

    public int CommentCount { get; set; }

    public int ViewCount { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public int TipCount { get; set; }

    public int Type { get; set; }

    public virtual ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();

    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

    public virtual ICollection<PostMedia> PostMedia { get; set; } = new List<PostMedia>();

    public virtual ICollection<PostTag> PostTags { get; set; } = new List<PostTag>();

    public virtual ICollection<Report> Reports { get; set; } = new List<Report>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<UserActivityLog> UserActivityLogs { get; set; } = new List<UserActivityLog>();
}

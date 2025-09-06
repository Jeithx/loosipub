using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserFollower
{
    public long Id { get; set; }

    public long FollowerId { get; set; }

    public long FollowedId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual User Followed { get; set; } = null!;

    public virtual User Follower { get; set; } = null!;
}

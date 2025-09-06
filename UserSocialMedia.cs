using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserSocialMedia
{
    public long Id { get; set; }

    public long SocialMediaId { get; set; }

    public long UserId { get; set; }

    public string Url { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual SocialMedia SocialMedia { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

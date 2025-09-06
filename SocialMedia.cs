using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class SocialMedia
{
    public long Id { get; set; }

    public string Logo { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public int LanguageId { get; set; }

    public virtual Language Language { get; set; } = null!;

    public virtual ICollection<UserSocialMedia> UserSocialMedia { get; set; } = new List<UserSocialMedia>();
}

using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Language
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string LangCode { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();

    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    public virtual ICollection<Faq> Faqs { get; set; } = new List<Faq>();

    public virtual ICollection<Gender> Genders { get; set; } = new List<Gender>();

    public virtual ICollection<SocialMedia> SocialMedia { get; set; } = new List<SocialMedia>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

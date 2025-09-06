using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Country
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public int LanguageId { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual Language Language { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class City
{
    public long Id { get; set; }

    public long CountryId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}

using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserCallPrice
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public decimal RatePerMinuute { get; set; }

    public int Currency { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual User User { get; set; } = null!;
}

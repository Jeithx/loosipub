using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Tip
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long SenderId { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public bool IsPay { get; set; }

    public virtual User Sender { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class CallPayment
{
    public long Id { get; set; }

    public long CallId { get; set; }

    public long PayerId { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaidDate { get; set; }

    public int PaymentStatus { get; set; }

    public virtual Call Call { get; set; } = null!;

    public virtual User Payer { get; set; } = null!;
}

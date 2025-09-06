using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Call
{
    public long Id { get; set; }

    public long CallerUserId { get; set; }

    public long ReceiverUserId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? TotalDurationInSeconds { get; set; }

    public decimal? TotalPrice { get; set; }

    public int Currency { get; set; }

    public int Status { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<CallPayment> CallPayments { get; set; } = new List<CallPayment>();

    public virtual User CallerUser { get; set; } = null!;

    public virtual User ReceiverUser { get; set; } = null!;
}

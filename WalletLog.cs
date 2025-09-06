using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class WalletLog
{
    public long Id { get; set; }

    public long WalletId { get; set; }

    public decimal Amount { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Wallet Wallet { get; set; } = null!;
}

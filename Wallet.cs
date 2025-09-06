using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Wallet
{
    public long Id { get; set; }

    public decimal Balance { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    public bool IsActive { get; set; }

    public long UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<WalletLog> WalletLogs { get; set; } = new List<WalletLog>();
}

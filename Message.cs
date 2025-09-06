using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Message
{
    public long Id { get; set; }

    public long SenderId { get; set; }

    public long ReceiverId { get; set; }

    public string? Message1 { get; set; }

    public string? FileUrl { get; set; }

    public DateTime SendDate { get; set; }

    public bool IsRead { get; set; }

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}

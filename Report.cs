using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Report
{
    public long Id { get; set; }

    public long ReporterId { get; set; }

    public long? ReportedUserId { get; set; }

    public long? PostId { get; set; }

    public string Reason { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Post? Post { get; set; }

    public virtual User? ReportedUser { get; set; }

    public virtual User Reporter { get; set; } = null!;

    public virtual ICollection<UserWarning> UserWarnings { get; set; } = new List<UserWarning>();
}

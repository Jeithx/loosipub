using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserWarning
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long ModeratorId { get; set; }

    public string Reason { get; set; } = null!;

    public long RelatedReportId { get; set; }

    public DateTime WarningDate { get; set; }

    public bool IsActive { get; set; }

    public virtual User Moderator { get; set; } = null!;

    public virtual Report RelatedReport { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

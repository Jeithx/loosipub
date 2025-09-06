using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserSubscription
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long SubscriptionPlanId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsCanceled { get; set; }

    public DateTime CreaitonDate { get; set; }

    public virtual UserSubscriptionPlan SubscriptionPlan { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserSubscriptionPlan
{
    public long Id { get; set; }

    public long ContentCreatorUserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public int DurationInDays { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public virtual User ContentCreatorUser { get; set; } = null!;

    public virtual ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
}

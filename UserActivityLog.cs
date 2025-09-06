using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserActivityLog
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long? PostId { get; set; }

    public long? TagId { get; set; }

    public long? ContentCreatorId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual User? ContentCreator { get; set; }

    public virtual Post? Post { get; set; }

    public virtual Tag? Tag { get; set; }

    public virtual User User { get; set; } = null!;
}

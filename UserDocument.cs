using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class UserDocument
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long DocumentId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Document Document { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

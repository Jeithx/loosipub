using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class PostTag
{
    public long Id { get; set; }

    public long PostId { get; set; }

    public long TagId { get; set; }

    public virtual Post Post { get; set; } = null!;

    public virtual Tag Tag { get; set; } = null!;
}

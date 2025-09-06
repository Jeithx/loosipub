using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class SearchLog
{
    public long Id { get; set; }

    public string SearchKey { get; set; } = null!;

    public long UserId { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }
}

using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Log
{
    public long Id { get; set; }

    public string Action { get; set; } = null!;

    public string Service { get; set; } = null!;

    public DateTime LogDate { get; set; }

    public string? Exception { get; set; }
}

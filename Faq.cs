using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Faq
{
    public long Id { get; set; }

    public int LanguageId { get; set; }

    public int TypeId { get; set; }

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Language Language { get; set; } = null!;
}

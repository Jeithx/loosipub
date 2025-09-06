using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Document
{
    public long Id { get; set; }

    public int LanguageId { get; set; }

    public string Name { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool IsActive { get; set; }

    public virtual Language Language { get; set; } = null!;

    public virtual ICollection<UserDocument> UserDocuments { get; set; } = new List<UserDocument>();
}

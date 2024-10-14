using System;
using System.Collections.Generic;

namespace Library_Management_System.Models;

public partial class Language
{
    public int Id { get; set; }

    public string? BookLanguage { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}

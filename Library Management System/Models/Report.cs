using System;
using System.Collections.Generic;

namespace Library_Management_System.Models;

public partial class Report
{
    public int Id { get; set; }

    public string ReportType { get; set; } = null!;

    public DateTime? GeneratedDate { get; set; }

    public string? ReportData { get; set; } = null!;

    public string Description { get; set; } = null!;

}

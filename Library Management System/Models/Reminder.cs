using System;
using System.Collections.Generic;

namespace Library_Management_System.Models;

public partial class Reminder
{
    public int Id { get; set; }

    public int? BookId { get; set; }

    public int? UserId { get; set; }

    public DateTime? ReminderDate { get; set; }

    public string ReminderMessage { get; set; } = null!;

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}

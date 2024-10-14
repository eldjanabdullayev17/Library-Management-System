using System;
using System.Collections.Generic;

namespace Library_Management_System.Models;

public partial class Notification
{
    public int Id { get; set;}

    public int? UserId { get; set; }

    public string NotificationMessage { get; set; } = null!;

    public DateTime? NotificationDate { get; set; }

    public int? BookId { get; set; }

    public int? EventId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Event? Event { get; set; }

    public virtual User? User { get; set; }
}

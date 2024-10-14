using System;
using System.Collections.Generic;

namespace Library_Management_System.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public DateTime? ReservationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public int? BookId { get; set; }

    public int? UserId { get; set; }

    public bool Active { get; set; } = true;

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}

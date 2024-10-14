using System;
using System.Collections.Generic;

namespace Library_Management_System.Models;

public partial class Rental
{
    public int Id { get; set; }

    public DateTime RentalDate { get; set; }

    public DateTime DueDate { get; set; }

    public bool Status { get; set; }

    public int? BookId { get; set; }

    public int? UserId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual User? User { get; set; }
}

using System;
using System.Collections.Generic;

namespace Library_Management_System.Models;

public partial class Book
{
    public int Id { get; set; }

    public string BookTitle { get; set; } = null!;

    public double BookPrice { get; set; }

    public string BookImg { get; set; } = null!;

    public int BookPage { get; set; }

    public DateTime? BookPublicationYear { get; set; }

    public int BookInventoryCount { get; set; }

    public int? AuthorId { get; set; }

    public int? CategoryId { get; set; }

    public int? LanguageId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Language? Language { get; set; }

    public virtual ICollection<NotificationBook> NotificationBooks { get; set; } = new List<NotificationBook>();

    public virtual ICollection<LogTable> LogTables { get; set; } = new List<LogTable>();

    public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

}

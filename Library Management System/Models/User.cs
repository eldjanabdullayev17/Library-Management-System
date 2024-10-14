using System;
using System.Collections.Generic;

namespace Library_Management_System.Models;

public partial class User
{
    public int Id { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? RoleId { get; set; }

    public bool Active { get; set; } = true;
    public string Email { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

	public virtual ICollection<NotificationBook> NotificationBooks { get; set; } = new List<NotificationBook>();

	public virtual ICollection<LogTable> LogTables { get; set; } = new List<LogTable>();

	public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();

    public virtual ICollection<Rental> Rentals { get; set; } = new List<Rental>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

	public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

	public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

}

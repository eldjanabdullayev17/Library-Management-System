using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Models;

public partial class OnlineLibraryManagementSystemContext : DbContext
{
    public OnlineLibraryManagementSystemContext()
    {
    }

    public OnlineLibraryManagementSystemContext(DbContextOptions<OnlineLibraryManagementSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<NotificationBook> NotificationBooks { get; set; }

    public virtual DbSet<NotificationEvent> NotificationEvents { get; set; }

    public virtual DbSet<Reminder> Reminders { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<LogTable> LogTables { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
 #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=USER\\SQLKODLAR;Database=Online_Library_Management_System;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Authors__3214EC0784924E20");

            entity.Property(e => e.AuthorBirthDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.AuthorName).HasMaxLength(50);
            entity.Property(e => e.AuthorSurname).HasMaxLength(50);
            entity.Property(e => e.Nationality).HasMaxLength(100);
        });

        modelBuilder.Entity<LogTable>(entity =>
        {
			entity.HasKey(e => e.Id).HasName("PK__LogTable__3214EC078938CC04");
			entity.Property(e => e.LogDate).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Book).WithMany(p => p.LogTables)
			   .HasForeignKey(d => d.BookId)
			   .OnDelete(DeleteBehavior.Cascade)
			   .HasConstraintName("FK__LogTables__BookI__7849DB76");

			entity.HasOne(d => d.User).WithMany(p => p.LogTables)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK__LogTables__UserI__793DFFAF");

		});

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07280CF4E5");

            entity.Property(e => e.BookPublicationYear).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.BookTitle).HasMaxLength(100);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Books__AuthorId__0A9D95DB");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Books__CatedoryI__0B91BA14");

            entity.HasOne(d => d.Language).WithMany(p => p.Books)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Books__LanguageI__0C85DE4D");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07869A52C2");

            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Events__3214EC071272E1C4");

            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.EventName).HasMaxLength(100);
            entity.Property(e => e.Location).HasMaxLength(150);

            entity.HasOne(d => d.User).WithMany(p => p.Events)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Events__UserId__25518C17");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Language__3214EC079EF63FEE");

            entity.Property(e => e.BookLanguage).HasMaxLength(30);
        });

        modelBuilder.Entity<NotificationBook>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07094BF82A");

            entity.Property(e => e.NotificationDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Book).WithMany(p => p.NotificationBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notificat__BookI__681373AD");

            entity.HasOne(d => d.User).WithMany(p => p.NotificationBooks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Notificat__UserI__662B2B3B");
        });

		modelBuilder.Entity<NotificationEvent>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC07E9D5CB2E");

			entity.Property(e => e.NotificationDate).HasDefaultValueSql("(getdate())");

			entity.HasOne(d => d.Event).WithMany(p => p.NotificationEvents)
				.HasForeignKey(d => d.EventId)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK__Notificat__Event__634EBE90");

		});

		modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Remender__3214EC07FC4B42BC");

            entity.HasOne(d => d.Book).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Remenders__BookI__2DE6D218");

            entity.HasOne(d => d.User).WithMany(p => p.Reminders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Remenders__UserI__2EDAF651");
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rentals__3214EC07FFE3BFD0");

			entity.Property(e => e.Status).HasDefaultValueSql("1");

			entity.HasOne(d => d.Book).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Rentals__BookId__2180FB33");

            entity.HasOne(d => d.User).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Rentals__UserId__22751F6C");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reports__3214EC071361FD08");

            entity.Property(e => e.GeneratedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ReportType).HasMaxLength(100);

        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reservat__3214EC073CE306A3");

            entity.Property(e => e.ExpirationDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ReservationDate).HasDefaultValueSql("(getdate())");
			entity.Property(e => e.Active).HasDefaultValueSql("1");

			entity.HasOne(d => d.Book).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__BookI__1CBC4616");

            entity.HasOne(d => d.User).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reservati__UserI__1DB06A4F");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ratings__3214EC07893EB70F");

			entity.HasOne(d => d.Book).WithMany(p => p.Ratings)
				.HasForeignKey(d => d.BookId)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK__Ratings__BookId__59C55456");

			entity.HasOne(d => d.User).WithMany(p => p.Ratings)
				.HasForeignKey(d => d.UserId)
				.OnDelete(DeleteBehavior.Cascade)
				.HasConstraintName("FK__Ratings__UserId__5AB9788F");
		});

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3214EC073E5D9C06");

            entity.Property(e => e.ReviewDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reviews__BookId__55009F39");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reviews__UserId__55F4C372");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC079F8D2BE4");

            entity.Property(e => e.RoleName).HasMaxLength(30);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0771813625");

            entity.Property(e => e.Firstname).HasMaxLength(50);
            entity.Property(e => e.Lastname).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(64);
            entity.Property(e => e.Username).HasMaxLength(50);
            entity.Property(e => e.Active).HasDefaultValueSql("1");


			entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Users__RoleId__114A936A");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserLogi__3214EC07EBA304CF");
            entity.Property(e => e.UserLoginDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK__UserLogin__UserI__395884C4");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

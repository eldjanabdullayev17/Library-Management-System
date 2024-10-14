using Library_Management_System.DTOs.Notification;
using Library_Management_System.Models;
using Library_Management_System.Exceptions;
using Microsoft.EntityFrameworkCore;
using Library_Management_System.Validation;
using Library_Management_System.Services.v1.Interfaces;

namespace Library_Management_System.Services.v1.Service
{
    public class NotificationService : INotificationService
    {
        private readonly OnlineLibraryManagementSystemContext _context;
        public NotificationService(OnlineLibraryManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<AddEventNotificationDTO> addEventNotificationAsync(AddEventNotificationDTO notification)
        {
            IsValid.IsValidId(notification.EventId);

            var eventExists = await _context.Events.AnyAsync(b => b.Id == notification.EventId);

            if (!eventExists)
            {
                throw new LibraryManagementSystemException($"Id-si {notification.EventId} olan event tapılmadı.");
            }

            if (string.IsNullOrEmpty(notification.NotificationMessage) && string.IsNullOrWhiteSpace(notification.NotificationMessage))
            {
                throw new LibraryManagementSystemException("Bildiriş mətni düzgün deyil.");
            }

            if (notification.NotificationDate > DateTime.Now)
            {
                throw new LibraryManagementSystemException("Bildiriş tarixi düzgün deyil.");
            }

            var newEventNotification = new NotificationEvent
            {
                NotificationMessage = notification.NotificationMessage,
                NotificationDate = notification.NotificationDate,
                EventId = notification.EventId
            };

            await _context.NotificationEvents.AddAsync(newEventNotification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<AddNewBookNotificationDTO> addNewBookNotificationAsync(AddNewBookNotificationDTO notification)
        {
            IsValid.IsValidId(notification.BookId);
            IsValid.IsValidId(notification.UserId);

            var bookExists = await _context.Books.AnyAsync(b => b.Id == notification.BookId);
            var userExists = await _context.Users.AnyAsync(u => u.Id == notification.UserId);

            if (!bookExists && !userExists)
            {
                throw new LibraryManagementSystemException($"Id-si {notification.BookId} olan kitab və Id-si {notification.UserId} olan istifadəçi tapılmadı.");
            }

            if (!bookExists)
            {
                throw new LibraryManagementSystemException($"Id-si {notification.BookId} olan kitab tapılmadı.");
            }

            if (!userExists)
            {
                throw new LibraryManagementSystemException($"Id-si {notification.UserId} olan istifadəçi tapılmadı.");
            }

            if (string.IsNullOrEmpty(notification.NotificationMessage) && string.IsNullOrWhiteSpace(notification.NotificationMessage))
            {
                throw new LibraryManagementSystemException("Bildiriş mətni düzgün deyil.");
            }

            if (notification.NotificationDate > DateTime.Now)
            {
                throw new LibraryManagementSystemException("Bildiriş tarixi düzgün deyil.");
            }

            var newBookNotification = new NotificationBook
            {
                UserId = notification.UserId,
                NotificationMessage = notification.NotificationMessage,
                NotificationDate = notification.NotificationDate,
                BookId = notification.BookId
            };

            await _context.NotificationBooks.AddAsync(newBookNotification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<IEnumerable<GetNotificationsByUserDTO>> getNotificationsByUserAsync(int userId)
        {
            IsValid.IsValidId(userId);

            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);

            if (!userExists)
            {
                throw new LibraryManagementSystemException($"Id-si {userId} olan istifadəçi tapılmadı.");
            }

            var notificationsEvent = await _context.Events
                .Include(n => n.User)
                .Include(n => n.NotificationEvents)
                .Where(n => n.UserId == userId).ToListAsync();


            var notificationsBook = await _context.NotificationBooks
                .Include(x => x.User)
                .Where(x => x.UserId == userId).ToListAsync();


            if (!notificationsBook.Any() && !notificationsEvent.Any())
            {
                throw new LibraryManagementSystemException("Bildiris yoxdur.");
            }

            return notificationsBook.Select(n => new GetNotificationsByUserDTO
            {
                Id = n.Id,
                UserName = n.User?.Username,
                NotificationMessage = n.NotificationMessage,
                NotificationDate = n.NotificationDate,
            }).ToList();
        }

        public async Task removeNotificationAsync(int notificationId)
        {
            IsValid.IsValidId(notificationId);

            var notification = await _context.NotificationBooks.SingleOrDefaultAsync(x => x.Id == notificationId);

            if (notification is null)
            {
                throw new LibraryManagementSystemException("Bildiris yoxdur.");
            }
            else
            {
                _context.NotificationBooks.Remove(notification);
                await _context.SaveChangesAsync();
            }
        }
    }
}

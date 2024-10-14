using Library_Management_System.DTOs.Reminder;
using Library_Management_System.Exceptions;
using Library_Management_System.Models;
using Library_Management_System.Services.v1.Interfaces;
using Library_Management_System.Validation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

namespace Library_Management_System.Services.v1.Service
{
    public class ReminderService : IReminderService
    {
        private readonly OnlineLibraryManagementSystemContext _context;
		private readonly IConfiguration _configuration;
		public ReminderService(OnlineLibraryManagementSystemContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AddReminderDTO> addReminderAsync(AddReminderDTO reminder)
        {
            IsValid.IsValidId(reminder.BookId);
            IsValid.IsValidId(reminder.UserId);

            var book = await _context.Books.AnyAsync(b => b.Id == reminder.BookId);
            var user = await _context.Users.AnyAsync(u => u.Id == reminder.UserId);

            if (!book && !user)
            {
                throw new LibraryManagementSystemException($"Id-si {reminder.BookId} olan kitab və Id-si {reminder.UserId} olan istifadəçi tapılmadı.");
            }

            if (!book)
            {
                throw new LibraryManagementSystemException($"Id-si {reminder.BookId} olan kitab tapılmadı.");
            }

            if (!user)
            {
                throw new LibraryManagementSystemException($"Id-si {reminder.UserId} olan istifadəçi tapılmadı.");
            }

            if (reminder.ReminderDate > DateTime.Now)
            {
                throw new LibraryManagementSystemException("Xatırlatma vaxtı gələcəkdə ola bilməz.");
            }

            var newReminder = new Reminder
            {
                BookId = reminder.BookId,
                UserId = reminder.UserId,
                ReminderDate = reminder.ReminderDate,
                ReminderMessage = $"Id-si {reminder.BookId} olan kitabı qaytarmalısınız!"
            };

            await _context.Reminders.AddAsync(newReminder);
            await _context.SaveChangesAsync();
            return reminder;
        }

        public async Task<GetRemindersByUserDTO> getRemindersByUserAsync(int userId)
        {
            IsValid.IsValidId(userId);

            var reminder = await _context.Users
                .Include(r => r.Reminders)
                .SingleOrDefaultAsync(r => r.Id == userId);

            if (reminder is null)
            {
                throw new LibraryManagementSystemException("İstifadəçi tapılmadı.");
            }
            else
            {
                var remindersByUser = new GetRemindersByUserDTO
                {
                    ReservationId = reminder.Id,
                    UserName = reminder.Username,
                    Reminders = reminder.Reminders.Select(b => b.ReminderMessage).ToList()
                };
                if (!remindersByUser.Reminders.Any())
                {
                    throw new LibraryManagementSystemException("Bu istifadəçi üçün xatırlatmalar yoxdur.");
                }

                return remindersByUser;
            }
        }

        public async Task<IEnumerable<ReminderDTO>> getUpcomingRemindersAsync(DateTime date)
        {
            var reminders = await _context.Reminders
            .Where(r => r.ReminderDate > date)
            .ToListAsync();

            if (!reminders.Any())
            {
                throw new LibraryManagementSystemException($"{date} bu vaxtdan sonra xatırlatma yoxdur.");
            }

            var reminder = reminders.Select(r => new ReminderDTO
            {
                ReminderId = r.Id,
                BookId = r.BookId,
                UserId = r.UserId,
                ReminderDate = r.ReminderDate,
                ReminderMessage = r.ReminderMessage
            }).ToList();

            return reminder;
        }

        public async Task<bool> removeReminderAsync(int reminderId)
        {
            var reminder = await _context.Reminders.SingleOrDefaultAsync(r => r.Id == reminderId);
            if (reminder is null)
            {
                return false;
            }
            else
            {
                _context.Reminders.Remove(reminder);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> sendReminderNotificationAsync(int userId, string message)
        {
            IsValid.IsValidId(userId);

            var user = await _context.Users.FindAsync(userId);


            if (user != null)
            {
				await SendEmailAsync(user.Email, "Bildiriş", message);
                return true;
            }

            return false;
        }

		private async Task SendEmailAsync(string to, string subject, string body)
		{
			// SMTP ayarlarını konfiqurasiya faylından (appsettings.json) oxuyun
			var smtpSettings = _configuration.GetSection("Smtp");

			if (!int.TryParse(smtpSettings["Port"], out int smtpPort))
			{
				throw new ArgumentException("SMTP Port düzgün konfiqurasiya edilməyib.");
			}

			bool enableSSL = bool.TryParse(smtpSettings["EnableSSL"], out enableSSL) && enableSSL;

			string smtpHost = smtpSettings["Host"];
			string smtpUsername = smtpSettings["Username"];
			string smtpPassword = smtpSettings["Password"];

			if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUsername) || string.IsNullOrEmpty(smtpPassword))
			{
				throw new ArgumentNullException("SMTP konfiqurasiya məlumatları tam deyil.");
			}

			// SMTP müştəri obyektini yaradın
			using (var smtpClient = new SmtpClient(smtpHost))
			{
				smtpClient.Port = smtpPort;
				smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
				smtpClient.EnableSsl = enableSSL;

				// E-poçt mesajını yaradın
				var mailMessage = new MailMessage
				{
					From = new MailAddress(smtpUsername), // Göndərən e-poçt adresi
					Subject = subject,                    // E-poçt başlığı
					Body = body,                          // E-poçt mətni
					IsBodyHtml = true                     // HTML formatında göndərmək üçün
				};

				mailMessage.To.Add(to); // Alan şəxsin e-poçt adresi
				// E-poçtu asinxron göndərin
				await smtpClient.SendMailAsync(mailMessage);
				
			}
		}

	}
}

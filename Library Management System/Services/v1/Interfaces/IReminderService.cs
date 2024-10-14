using Library_Management_System.DTOs.Reminder;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface IReminderService
    {
        // İstifadəçiyə kitabın qaytarılması üçün xatırlatma əlavə edir.
        Task<AddReminderDTO> addReminderAsync(AddReminderDTO reminder);

        // Xatırlatmanı silir.
        Task<bool> removeReminderAsync(int reminderId);

        // Müəyyən istifadəçi üçün bütün xatırlatmaları əldə edir.
        Task<GetRemindersByUserDTO> getRemindersByUserAsync(int userId);

        // Gələcəkdəki xatırlatmaları əldə edir (məsələn,müəyyən bir tarixdən sonrakı).
        Task<IEnumerable<ReminderDTO>> getUpcomingRemindersAsync(DateTime date);

        // İstifadəçiyə e-poçt, SMS və ya tətbiq içi bildiriş vasitəsilə xatırlatma göndərir.
        Task<bool> sendReminderNotificationAsync(int userId, string message);
    }
}

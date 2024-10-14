using Library_Management_System.DTOs.Notification;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface INotificationService
    {
        // İstifadəçilərə yeni kitablar haqqında bildiriş əlavə edir.
        Task<AddNewBookNotificationDTO> addNewBookNotificationAsync(AddNewBookNotificationDTO notification);

        // İstifadəçilərə tədbirlər haqqında bildiriş əlavə edir.
        Task<AddEventNotificationDTO> addEventNotificationAsync(AddEventNotificationDTO notification);

        // Müəyyən istifadəçi üçün bütün bildirişləri əldə edir.
        Task<IEnumerable<GetNotificationsByUserDTO>> getNotificationsByUserAsync(int userId);

        // Bildirişi silir
        Task removeNotificationAsync(int notificationId);
    }
}

using Library_Management_System.DTOs.Book;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface IBookService
    {

        // Müəyyən kitab haqqında məlumatları əldə edir.
        Task<GetBookByIdDTO> getBookByIdAsync(int bookId);

        // Yeni kitab əlavə edir.
        Task<AddBookDTO> addBookAsync(AddBookDTO book);

        // Kitabı silir.
        Task<bool> deleteBookAsync(int bookId);

        // Mövcud kitabın məlumatlarını yeniləyir.
        Task<bool> updateBookAsync(int bookId, UpdateBookDTO newBook);

        // Bütün kitabları əldə edir.
        Task<IEnumerable<GetAllBooksDTO>> getAllBooksAsync();

        // Kitabları axtarış meyarlarına uyğun olaraq axtarır.
        Task<IEnumerable<SearchBooksDTO>> searchBooksAsync(string searchText);

        // Kitabları müəyyən kriteriyalara görə filtrləyir.
        Task<IEnumerable<FilterBooksDTO>> filterBooksAsync(QueryObject query);

        // Kitabların mövcud sayını izləyir və göstərir.
        Task<IEnumerable<GetBookInventoryDTO>> getBookInventoryAsync();

    }
}

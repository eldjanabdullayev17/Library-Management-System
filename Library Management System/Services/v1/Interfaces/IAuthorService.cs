using Library_Management_System.DTOs.Author;

namespace Library_Management_System.Services.v1.Interfaces
{
    public interface IAuthorService
    {

        // Yeni müəllif əlavə edir.
        Task<AddAuthorDTO> addAuthorAsync(AddAuthorDTO author);

        // Mövcud müəllifin məlumatlarını yeniləyir.
        Task<bool> updateAuthorAsync(int authorId, UpdateAuthorDTO newAuthor);

        // Müəllifi silir.
        Task<bool> deleteAuthorAsync(int authorId);

        // Müəyyən müəllif haqqında məlumatları əldə edir.
        Task<GetAuthorByIdDTO> getAuthorByIdAsync(int authorId);

        // Bütün müəllifləri əldə edir.
        Task<IEnumerable<GetAllAuthorsDTO>> getAllAuthorsAsync();

        // Müəllifin adına əsasən məlumatları əldə edir.
        Task<IEnumerable<GetAuthorByNameDTO>> getAuthorByNameAsync(string authorName);

        // Müəllifin kitabını silir.
        Task<bool> removeBookFromAuthorAsync(int authorId);

        // Müəllifə aid kitabları əldə edir.
        Task<GetBooksByAuthorDTO> getBooksByAuthorAsync(int authorId);


    }
}

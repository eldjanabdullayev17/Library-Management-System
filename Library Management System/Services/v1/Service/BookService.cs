using System.Linq;
using Library_Management_System.DTOs.Book;
using Library_Management_System.Exceptions;
using Library_Management_System.Models;
using Library_Management_System.Services.v1.Interfaces;
using Library_Management_System.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Library_Management_System.Services.v1.Service
{
    public class BookService : IBookService
    {
        private readonly OnlineLibraryManagementSystemContext _context;

        public BookService(OnlineLibraryManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<AddBookDTO> addBookAsync(AddBookDTO book)
        {
            var user = await _context.UserLogins.Include(x => x.User).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var superAdminId = await _context.Roles.SingleOrDefaultAsync(s => s.RoleName.ToLower() == "user");

            if (user?.User?.RoleId == superAdminId?.Id)
            {
                throw new LibraryManagementSystemException("Sizin səlahiyyətiniz yoxdur!");
            }

            if (string.IsNullOrEmpty(book.BookTitle) || string.IsNullOrWhiteSpace(book.BookTitle))
            {
                throw new LibraryManagementSystemException("Kitab başlığı düzgün verilməyib.");
            }

            if (book.BookPrice <= 0)
            {
                throw new LibraryManagementSystemException("Kitabın qiyməti 0-dan böyük olmalıdır.");
            }

            if (!IsValid.IsValidImageFormat(book.BookImg))
            {
                throw new LibraryManagementSystemException("Kitabın şəkli düzgün formatda deyil.");
            }

            if (book.BookInventoryCount < 0)
            {
                throw new LibraryManagementSystemException("Kitabın sayı mənfi ola bilməz.");
            }

            if (book.BookPage <= 0)
            {
                throw new LibraryManagementSystemException("Kitabın səhifə sayı düzgün daxil edilməyib.");
            }

            if (book.BookPublicationYear > DateTime.Now)
            {
                throw new LibraryManagementSystemException("Kitabın nəşr ili düzgün deyil.");
            }

            IsValid.IsValidId(book.AuthorId);
            IsValid.IsValidId(book.CategoryId);
            IsValid.IsValidId(book.LanguageId);

            var author = await _context.Authors.AnyAsync(a => a.Id == book.AuthorId);
            var category = await _context.Categories.AnyAsync(c => c.Id == book.CategoryId);
            var language = await _context.Languages.AnyAsync(l => l.Id == book.LanguageId);

            if (!author)
            {
                throw new LibraryManagementSystemException($"İd-si {book.AuthorId} olan müəllif yoxdur");
            }

            if (!category)
            {
                throw new LibraryManagementSystemException($"İd-si {book.CategoryId} olan kateqori yoxdur");
            }

            if (!language)
            {
                throw new LibraryManagementSystemException($"İd-si {book.LanguageId} olan dil yoxdur");
            }

            var newBook = new Book()
            {
                BookTitle = book.BookTitle,
                BookPrice = book.BookPrice,
                BookImg = book.BookImg,
                BookInventoryCount = book.BookInventoryCount,
                BookPage = book.BookPage,
                BookPublicationYear = book.BookPublicationYear,
                AuthorId = book.AuthorId,
                CategoryId = book.CategoryId,
                LanguageId = book.LanguageId,
            };

            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();
            return book;
        }

        public async Task<bool> deleteBookAsync(int bookId)
        {
            var user = await _context.UserLogins.Include(x => x.User).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var superAdminId = await _context.Roles.SingleOrDefaultAsync(s => s.RoleName.ToLower() == "user");

            if (user?.User?.RoleId == superAdminId?.Id)
            {
                throw new LibraryManagementSystemException("Sizin səlahiyyətiniz yoxdur!");
            }

            IsValid.IsValidId(bookId);

            var book = await _context.Books.SingleOrDefaultAsync(x => x.Id == bookId);

            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<FilterBooksDTO>> filterBooksAsync(QueryObject query)
        {
            var books = _context.Books.Include(x => x.Author).Include(x => x.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Author))
            {
                books = books
                    .Where(x => string.Concat(x.Author.AuthorName.ToLower(), " ", x.Author.AuthorSurname.ToLower()) == query.Author.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                books = books.Where(x => x.Category.CategoryName.ToLower() == query.Category.ToLower());
            }

            return await books.Select(x => new FilterBooksDTO
            {
                Id = x.Id,
                BookTitle = x.BookTitle,
                BookPrice = x.BookPrice,
                Author = $"{x.Author.AuthorName} {x.Author.AuthorSurname}",
                Category = x.Category.CategoryName
            }).ToListAsync();
        }

        public async Task<IEnumerable<GetAllBooksDTO>> getAllBooksAsync()
        {
            return await _context.Books
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.Language)
                .Select(x => new GetAllBooksDTO
                {
                    Id = x.Id,
                    BookTitle = x.BookTitle,
                    BookPrice = x.BookPrice,
                    BookPublicationYear = x.BookPublicationYear,
                    Author = $"{x.Author.AuthorName} {x.Author.AuthorSurname}",
                    Category = x.Category.CategoryName,
                    Language = x.Language.BookLanguage
                }).ToListAsync();
        }

        public async Task<GetBookByIdDTO> getBookByIdAsync(int bookId)
        {

            IsValid.IsValidId(bookId);

            var book = await _context.Books
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Include(x => x.Language)
                .SingleOrDefaultAsync(x => x.Id == bookId);

            if (book != null)
            {
                return new GetBookByIdDTO
                {
                    Id = book.Id,
                    BookTitle = book.BookTitle,
                    BookPrice = book.BookPrice,
                    BookImg = book.BookImg,
                    BookPublicationYear = book.BookPublicationYear,
                    BookPage = book.BookPage,
                    BookInventoryCount = book.BookInventoryCount,
                    Author = $"{book?.Author?.AuthorName} {book?.Author?.AuthorSurname}",
                    Category = book.Category?.CategoryName,
                    Language = book.Language?.BookLanguage
                };
            }
            else
            {
                throw new LibraryManagementSystemException("Kitab tapılmadı.");
            }
        }

        public async Task<IEnumerable<GetBookInventoryDTO>> getBookInventoryAsync()
        {
            return await _context.Books.Select(book => new GetBookInventoryDTO
            {
                Id = book.Id,
                BookTitle = book.BookTitle,
                BookInventoryCount = book.BookInventoryCount
            }).ToListAsync();
        }

        public async Task<IEnumerable<SearchBooksDTO>> searchBooksAsync(string keyword)
        {
            var books = _context.Books.Include(x => x.Author).Include(x => x.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword) || !string.IsNullOrEmpty(keyword))
            {

                return await books.Where(book => book.BookTitle.ToLower().Contains(keyword.ToLower()) ||
                            book.Author.AuthorName.ToLower().Contains(keyword.ToLower()) ||
                            book.Category.CategoryName.ToLower().Contains(keyword.ToLower()))
                            .Select(x => new SearchBooksDTO
                            {
                                Id = x.Id,
                                BookTitle = x.BookTitle,
                                BookPrice = x.BookPrice,
                                Author = $"{x.Author.AuthorName} {x.Author.AuthorSurname}",
                                Category = x.Category.CategoryName
                            }).ToListAsync();
            }
            else
            {
                throw new LibraryManagementSystemException("Axtarış üçün bir term daxil edilməlidir.");
            }
        }

        public async Task<bool> updateBookAsync(int bookId, UpdateBookDTO newBook)
        {
            var user = await _context.UserLogins.Include(x => x.User).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var superAdminId = await _context.Roles.SingleOrDefaultAsync(s => s.RoleName.ToLower() == "user");

            if (user?.User?.RoleId == superAdminId?.Id)
            {
                throw new LibraryManagementSystemException("Sizin səlahiyyətiniz yoxdur!");
            }

            IsValid.IsValidId(bookId);

            if (string.IsNullOrEmpty(newBook.BookTitle) || string.IsNullOrWhiteSpace(newBook.BookTitle))
            {
                throw new LibraryManagementSystemException("Kitab başlığı düzgün verilməyib.");
            }

            if (newBook.BookPrice <= 0)
            {
                throw new LibraryManagementSystemException("Kitabın qiyməti 0-dan böyük olmalıdır");
            }

            if (string.IsNullOrEmpty(newBook.BookImg) || !IsValid.IsValidImageFormat(newBook.BookImg))
            {
                throw new LibraryManagementSystemException("Kitabın şəkli düzgün formatda deyil");
            }

            if (newBook.BookInventoryCount < 0)
            {
                throw new LibraryManagementSystemException("Kitabın sayı mənfi ola bilməz");
            }

            if (newBook.BookPage <= 0)
            {
                throw new LibraryManagementSystemException("Kitabın səhifə sayı düzgün daxil edilməyib");
            }

            if (newBook.BookPublicationYear > DateTime.Now)
            {
                throw new LibraryManagementSystemException("Kitabın nəşr ili gələcəkdə ola bilməz");
            }

            var book = await _context.Books.SingleOrDefaultAsync(x => x.Id == bookId);

            if (book != null)
            {
                book.BookTitle = newBook.BookTitle;
                book.BookPrice = newBook.BookPrice;
                book.BookImg = newBook.BookImg;
                book.BookPage = newBook.BookPage;
                book.BookPublicationYear = newBook.BookPublicationYear;
                book.BookInventoryCount = newBook.BookInventoryCount;

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}

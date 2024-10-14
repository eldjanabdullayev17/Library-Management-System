using System.Net;
using Library_Management_System.DTOs.Author;
using Library_Management_System.Exceptions;
using Library_Management_System.Models;
using Library_Management_System.Services.v1.Interfaces;
using Library_Management_System.Validation;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Services.v1.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly OnlineLibraryManagementSystemContext _context;
        public AuthorService(OnlineLibraryManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<AddAuthorDTO> addAuthorAsync(AddAuthorDTO author)
        {
            if (string.IsNullOrEmpty(author.AuthorName) || string.IsNullOrWhiteSpace(author.AuthorName))
            {
                throw new LibraryManagementSystemException("Müəllifin adı düzgün verilməyib.");
            }

            if (string.IsNullOrEmpty(author.AuthorSurname) || string.IsNullOrWhiteSpace(author.AuthorSurname))
            {
                throw new LibraryManagementSystemException("Müəllifin soyadı düzgün verilməyib.");
            }

            if (author.AuthorBirthDate > DateTime.Now)
            {
                throw new LibraryManagementSystemException("Müəllifin doğum tarixi düzgün deyil.");
            }

            if (string.IsNullOrEmpty(author.AuthorBiography) || string.IsNullOrWhiteSpace(author.AuthorBiography))
            {
                throw new LibraryManagementSystemException("Müəllifin bioqrafiyası düzgün verilməyib.");
            }

            if (string.IsNullOrEmpty(author.Nationality) || string.IsNullOrWhiteSpace(author.Nationality))
            {
                throw new LibraryManagementSystemException("Müəllifin milliyyəti düzgün verilməyib.");
            }

            if (string.IsNullOrWhiteSpace(author.AuthorImg) || !IsValid.IsValidImageFormat(author.AuthorImg))
            {
                throw new LibraryManagementSystemException("Müəllifin şəkli düzgün formatda deyil.");
            }

            if (author.NumberOfBooks <= 0)
            {
                throw new LibraryManagementSystemException("Kitabın səhifə sayı düzgün daxil edilməyib.");
            }

            var newAuthor = new Author()
            {
                AuthorName = author.AuthorName,
                AuthorSurname = author.AuthorSurname,
                AuthorBirthDate = author.AuthorBirthDate,
                AuthorBiography = author.AuthorBiography,
                Nationality = author.Nationality,
                AuthorImg = author.AuthorImg,
                NumberOfBooks = author.NumberOfBooks
            };

            await _context.Authors.AddAsync(newAuthor);
            await _context.SaveChangesAsync();
            return author;

        }

        public async Task<bool> deleteAuthorAsync(int authorId)
        {
            IsValid.IsValidId(authorId);

            var author = await _context.Authors.SingleOrDefaultAsync(x => x.Id == authorId);

            if (author != null)
            {
                _context.Authors.Remove(author);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<GetAllAuthorsDTO>> getAllAuthorsAsync()
        {
            return await _context.Authors.Select(x => new GetAllAuthorsDTO
            {
                AuthorName = x.AuthorName,
                AuthorSurname = x.AuthorSurname,
                AuthorBirthDate = x.AuthorBirthDate,
                Nationality = x.Nationality,
                AuthorBiography = x.AuthorBiography
            }).ToListAsync();
        }

        public async Task<GetAuthorByIdDTO> getAuthorByIdAsync(int authorId)
        {
            IsValid.IsValidId(authorId);

            var author = await _context.Authors.SingleOrDefaultAsync(x => x.Id == authorId);

            if (author != null)
            {
                return new GetAuthorByIdDTO
                {
                    Id = author.Id,
                    AuthorName = author.AuthorName,
                    AuthorSurname = author.AuthorSurname,
                    AuthorBirthDate = author.AuthorBirthDate,
                    Nationality = author.Nationality,
                    AuthorBiography = author.AuthorBiography,
                    AuthorImg = author.AuthorImg,
                    NumberOfBooks = author.NumberOfBooks
                };
            }
            else
            {
                throw new LibraryManagementSystemException("Müəllif tapılmadı.");
            }
        }

        public async Task<IEnumerable<GetAuthorByNameDTO>> getAuthorByNameAsync(string author)
        {

            if (!string.IsNullOrEmpty(author) || !string.IsNullOrWhiteSpace(author))
            {
                return await _context.Authors
                    .Where(x => (x.AuthorName.ToLower() + " " + x.AuthorSurname.ToLower())
                    .Contains(author.ToLower()))
                    .Select(x => new GetAuthorByNameDTO
                    {
                        Id = x.Id,
                        AuthorName = x.AuthorName,
                        AuthorSurname = x.AuthorSurname,
                        AuthorBirthDate = x.AuthorBirthDate,
                        Nationality = x.Nationality,
                        AuthorBiography = x.AuthorBiography,
                        AuthorImg = x.AuthorImg,
                        NumberOfBooks = x.NumberOfBooks
                    }).ToListAsync();
            }
            else
            {
                throw new LibraryManagementSystemException("Axtarış üçün bir term daxil edilməlidir.");
            }
        }

        public async Task<GetBooksByAuthorDTO> getBooksByAuthorAsync(int authorId)
        {
            IsValid.IsValidId(authorId);

            var author = await _context.Authors
                .Include(x => x.Books)
                .SingleOrDefaultAsync(x => x.Id == authorId);

            if (author != null)
            {
                var books = new GetBooksByAuthorDTO
                {
                    AuthorId = authorId,
                    Author = $"{author.AuthorName} {author.AuthorSurname}",
                    Books = author.Books.Select(b => b.BookTitle).ToList()
                };

                if (!books.Books.Any())
                {
                    throw new LibraryManagementSystemException("Müəllifə aid kitab tapılmadı.");
                }

                return books;
            }
            else
            {
                throw new LibraryManagementSystemException("Müəllif tapılmadı.");
            }
        }

        public async Task<bool> removeBookFromAuthorAsync(int authorId)
        {
            IsValid.IsValidId(authorId);

            var author = await _context.Authors.AnyAsync(a => a.Id == authorId);

            if (!author)
            {
                throw new LibraryManagementSystemException($"İd-si {authorId} olan müəllif yoxdur");
            }

            var books = await _context.Books
                .Where(x => x.AuthorId == authorId)
                .ToListAsync();

            if (books.Any())
            {
                foreach (var book in books)
                {
                    _context.Books.Remove(book);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<bool> updateAuthorAsync(int authorId, UpdateAuthorDTO newAuthor)
        {
            IsValid.IsValidId(authorId);

            if (!string.IsNullOrEmpty(newAuthor.AuthorName) || !string.IsNullOrWhiteSpace(newAuthor.AuthorName))
            {
                throw new LibraryManagementSystemException("Müəllifin adı düzgün verilməyib.");
            }

            if (!string.IsNullOrEmpty(newAuthor.AuthorSurname) || !string.IsNullOrWhiteSpace(newAuthor.AuthorSurname))
            {
                throw new LibraryManagementSystemException("Müəllifin soyadı düzgün verilməyib.");
            }

            if (newAuthor.AuthorBirthDate > DateTime.Now)
            {
                throw new LibraryManagementSystemException("Müəllifin doğum tarixi düzgün deyil.");
            }

            if (!string.IsNullOrEmpty(newAuthor.AuthorBiography) || !string.IsNullOrWhiteSpace(newAuthor.AuthorBiography))
            {
                throw new LibraryManagementSystemException("Müəllifin bioqrafiyası düzgün verilməyib.");
            }

            if (!string.IsNullOrEmpty(newAuthor.Nationality) || !string.IsNullOrWhiteSpace(newAuthor.Nationality))
            {
                throw new LibraryManagementSystemException("Müəllifin milliyyəti düzgün verilməyib.");
            }

            if (string.IsNullOrEmpty(newAuthor.AuthorImg) || !IsValid.IsValidImageFormat(newAuthor.AuthorImg))
            {
                throw new LibraryManagementSystemException("Müəllifin şəkli düzgün formatda deyil.");
            }

            if (newAuthor.NumberOfBooks <= 0)
            {
                throw new LibraryManagementSystemException("Kitabın səhifə sayı düzgün daxil edilməyib.");
            }

            var author = await _context.Authors.SingleOrDefaultAsync(x => x.Id == authorId);

            if (author != null)
            {
                author.AuthorName = newAuthor.AuthorName;
                author.AuthorSurname = newAuthor.AuthorSurname;
                author.AuthorBirthDate = newAuthor.AuthorBirthDate;
                author.Nationality = newAuthor.Nationality;
                author.AuthorBiography = newAuthor.AuthorBiography;
                author.AuthorImg = newAuthor.AuthorImg;
                author.NumberOfBooks = newAuthor.NumberOfBooks;

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

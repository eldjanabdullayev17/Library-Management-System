using Library_Management_System.DTOs.Rating;
using Library_Management_System.Models;
using Library_Management_System.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Library_Management_System.Validation;
using Library_Management_System.Services.v1.Interfaces;

namespace Library_Management_System.Services.v1.Service
{
    public class RatingService : IRatingService
    {
        private readonly OnlineLibraryManagementSystemContext _context;
        public RatingService(OnlineLibraryManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<AddRatingDTO> addRatingAsync(AddRatingDTO rating)
        {
            if (rating.BookRating > 5 || rating.BookRating < 0)
            {
                throw new LibraryManagementSystemException("Kitabın reytinqi 1 və 5 arasında olmalıdır.");
            }

            IsValid.IsValidId(rating.BookId);
            IsValid.IsValidId(rating.UserId);

            var book = await _context.Books.AnyAsync(b => b.Id == rating.BookId);
            var user = await _context.Users.AnyAsync(u => u.Id == rating.UserId);

            if (!book && !user)
            {
                throw new LibraryManagementSystemException($"Id-si {rating.BookId} olan kitab və Id-si {rating.UserId} olan istifadəçi tapılmadı.");
            }

            if (!book)
            {
                throw new LibraryManagementSystemException($"Id-si {rating.BookId} olan kitab tapılmadı.");
            }

            if (!user)
            {
                throw new LibraryManagementSystemException($"Id-si {rating.UserId} olan istifadəçi tapılmadı.");
            }

            var newRating = new Rating
            {
                BookRating = rating.BookRating,
                BookId = rating.BookId,
                UserId = rating.UserId
            };

            var ratingLog = new LogTable
            {
                UserId = rating.UserId,
                BookId = rating.BookId,
                LogMessage = $"Istifadəçi kitaba reytinq verdi",
                LogDate = DateTime.Now
            };

            await _context.LogTables.AddAsync(ratingLog);
            await _context.Ratings.AddAsync(newRating);
            await _context.SaveChangesAsync();
            return rating;

        }

        public async Task<bool> deleteRatingAsync(int ratingId)
        {
            IsValid.IsValidId(ratingId);

            var rating = await _context.Ratings
                .SingleOrDefaultAsync(r => r.Id == ratingId);

            if (rating != null)
            {
				var ratingLog = new LogTable
				{
                    UserId = rating.UserId,
                    BookId= rating.BookId,
					LogMessage = $"İstifadəçi reytinqi silindi.",
					LogDate = DateTime.Now
				};

				await _context.LogTables.AddAsync(ratingLog);

				_context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<double> getAverageRatingAsync(int bookId)
        {
            IsValid.IsValidId(bookId);

            var book = await _context.Books.AnyAsync(b => b.Id == bookId);

            if (!book)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab yoxdur");
            }

            var rating = await _context.Ratings.Where(x => x.BookId == bookId).ToListAsync();

            if (rating.Any())
            {
                double avarageRating = (double)await _context.Ratings
                    .Where(r => r.BookId == bookId)
                    .AverageAsync(r => r.BookRating);
                return avarageRating;
            }
            else
            {
                throw new LibraryManagementSystemException($"Id- si {bookId} olan kitaba reytinq verilməyib.");
            }
        }

        public async Task<GetUserRatingDTO> getUserRatingAsync(int userId, int bookId)
        {
            IsValid.IsValidId(userId);
            IsValid.IsValidId(bookId);

            var book = await _context.Books.AnyAsync(b => b.Id == bookId);
            var user = await _context.Users.AnyAsync(u => u.Id == userId);

            if (!book && !user)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab və Id-si {userId} olan istifadəçi tapılmadı.");
            }

            if (!book)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab tapılmadı.");
            }

            if (!user)
            {
                throw new LibraryManagementSystemException($"Id-si {userId} olan istifadəçi tapılmadı.");
            }

            var rating = await _context.Ratings
                .Include(r => r.Book)
                .Include(r => r.User)
                .SingleOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId);

            if (rating != null)
            {
                return new GetUserRatingDTO
                {
                    UserName = rating.User.Username,
                    BookTitle = rating.Book.BookTitle,
                    Rating = rating.BookRating
                };
            }
            else
            {
                throw new LibraryManagementSystemException($"Id-si {userId} olan istifadəçi Id-si {bookId} olan kitaba reytinq verməyib.");
            }
        }

        public async Task<bool> updateRatingAsync(int ratingId, UpdateRatingDTO newRating)
        {
            IsValid.IsValidId(ratingId);

            if (newRating.BookRating > 5 || newRating.BookRating < 0)
            {
                throw new LibraryManagementSystemException("Kitabın reytinqi 1 və 5 arasında olmalıdır.");
            }

            var rating = await _context.Ratings.SingleOrDefaultAsync(r => r.Id == ratingId);

            if (rating != null)
            {
				var ratingLog = new LogTable
				{
					UserId = rating.UserId,
					BookId = rating.BookId,
					LogMessage = $"İstifadəçi reytinqini yenilədi.",
					LogDate = DateTime.Now
				};

				await _context.LogTables.AddAsync(ratingLog);

				rating.BookRating = newRating.BookRating;

                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<AddRatingAndReviewDTO> addRatingAndReviewAsync(AddRatingAndReviewDTO ratingAndReview)
        {
            if (ratingAndReview.Rating > 5 || ratingAndReview.Rating < 0)
            {
                throw new LibraryManagementSystemException("Kitabın reytinqi düzgün verilməyib.");
            }

            if (!string.IsNullOrEmpty(ratingAndReview.Review) || !string.IsNullOrWhiteSpace(ratingAndReview.Review))
            {
                throw new LibraryManagementSystemException("Rəy düzgün verilməyib.");
            }

            IsValid.IsValidId(ratingAndReview.UserId);
            IsValid.IsValidId(ratingAndReview.BookId);

            var book = await _context.Books.AnyAsync(b => b.Id == ratingAndReview.BookId);
            var user = await _context.Users.AnyAsync(u => u.Id == ratingAndReview.UserId);

            if (!book && !user)
            {
                throw new LibraryManagementSystemException($"Id-si {ratingAndReview.BookId} olan kitab və Id-si {ratingAndReview.UserId} olan istifadəçi tapılmadı.");
            }

            if (!book)
            {
                throw new LibraryManagementSystemException($"Id-si {ratingAndReview.BookId} olan kitab tapılmadı.");
            }

            if (!user)
            {
                throw new LibraryManagementSystemException($"Id-si {ratingAndReview.UserId} olan istifadəçi tapılmadı.");
            }

            var newRating = new Rating
            {
                BookId = ratingAndReview.BookId,
                UserId = ratingAndReview.UserId,
                BookRating = ratingAndReview.Rating
            };


            var newReview = new Review
            {
                BookId = ratingAndReview.BookId,
                UserId = ratingAndReview.UserId,
                ReviewText = ratingAndReview.Review,
                ReviewDate = DateTime.Now
            };

            await _context.Ratings.AddAsync(newRating);
            await _context.Reviews.AddAsync(newReview);
            await _context.SaveChangesAsync();
            return ratingAndReview;
        }

        public async Task<GetRatingsAndReviewsByBookDTO> getRatingsAndReviewsByBookAsync(int bookId)
        {
            IsValid.IsValidId(bookId);

            var book = await _context.Books
                .Include(r => r.Ratings)
                .Include(r => r.Reviews)
                .SingleOrDefaultAsync(r => r.Id == bookId);

            var ratingAndReview = new GetRatingsAndReviewsByBookDTO
            {
                BookTitle = book.BookTitle,
                Ratings = book.Ratings.Select(r => r.BookRating).ToList(),
                Reviews = book.Reviews.Select(r => r.ReviewText).ToList()
            };

            if (!ratingAndReview.Reviews.Any() && !ratingAndReview.Ratings.Any())
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitaba rəy və reytinq verilməyib.");
            }

            return ratingAndReview;

        }

        public async Task<GetRatingsAndReviewsByUserDTO> getRatingsAndReviewsByUserAsync(int userId)
        {
            IsValid.IsValidId(userId);

            var user = await _context.Users
                .Include(r => r.Ratings)
                .Include(r => r.Reviews)
                .SingleOrDefaultAsync(r => r.Id == userId);


            var ratingAndReview = new GetRatingsAndReviewsByUserDTO
            {
                UserName = user.Username,
                Ratings = user.Ratings.Select(r => r.BookRating).ToList(),
                Reviews = user.Reviews.Select(r => r.ReviewText).ToList()
            };

            if (!ratingAndReview.Reviews.Any() && !ratingAndReview.Ratings.Any())
            {
                throw new LibraryManagementSystemException($"Id-si {userId} olan istifadəçi rəy və reytinq yazmayıb.");
            }

            return ratingAndReview;

        }

        public async Task<bool> updateRatingAndReviewAsync(int bookId, int userId, UpdateRatingAndReviewDTO newUpdateRatingAndReview)
        {
            IsValid.IsValidId(userId);
            IsValid.IsValidId(bookId);

            var book = await _context.Books.AnyAsync(b => b.Id == bookId);
            var user = await _context.Users.AnyAsync(u => u.Id == userId);

            if (!book && !user)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab və Id-si {userId} olan istifadəçi tapılmadı.");
            }

            if (!book)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab tapılmadı.");
            }

            if (!user)
            {
                throw new LibraryManagementSystemException($"Id-si {userId} olan istifadəçi tapılmadı.");
            }

            if (newUpdateRatingAndReview.Rating > 5 || newUpdateRatingAndReview.Rating < 0)
            {
                throw new LibraryManagementSystemException("Kitabın reytinqi 1 və 5 arasında olmalıdır.");
            }

            if (string.IsNullOrEmpty(newUpdateRatingAndReview.Review) || string.IsNullOrWhiteSpace(newUpdateRatingAndReview.Review))
            {
                throw new LibraryManagementSystemException("Rəy düzgün verilməyib.");
            }


            var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId);

            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId);

            if (rating == null || review == null)
            {
                return false;
            }

            // Reytinq və rəyi yeniləyək

            rating.BookRating = newUpdateRatingAndReview.Rating;
            review.ReviewText = newUpdateRatingAndReview.Review;
            review.ReviewDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> deleteRatingAndReviewAsync(int bookId, int userId)
        {
            IsValid.IsValidId(userId);
            IsValid.IsValidId(bookId);

            var book = await _context.Books.AnyAsync(b => b.Id == bookId);
            var user = await _context.Users.AnyAsync(u => u.Id == userId);

            if (!book && !user)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab və Id-si {userId} olan istifadəçi tapılmadı.");
            }

            if (!book)
            {
                throw new LibraryManagementSystemException($"Id-si {bookId} olan kitab tapılmadı.");
            }

            if (!user)
            {
                throw new LibraryManagementSystemException($"Id-si {userId} olan istifadəçi tapılmadı.");
            }

            var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId);

            if (rating is null)
            {
                return false;
            }

            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.BookId == bookId && r.UserId == userId);

            _context.Ratings.Remove(rating);

            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

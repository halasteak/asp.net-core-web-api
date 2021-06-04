using my_books.Data.Models;
using my_books.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Data.Services
{
    public class BooksService
    {
        private AppDbContext _context;
        public BooksService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public void AddBookWithAuthors(BookVM book) {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.DateRead.HasValue ? book.DateRead : null,
                Rate = book.Rate,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                AddedDate = DateTime.Now,
                PublisherId = book.PublisherId,


            };
            _context.Books.Add(_book);
            _context.SaveChanges();


            foreach (var id in book.AuthorsIds)
            {
                var _bookAuthor = new Book_Author()
                {
                    BookId = _book.Id,
                    AuthorId = id
                };
                _context.Books_Authors.Add(_bookAuthor);
                _context.SaveChanges();
            }
        }

        public List<Book> GetAllBooks() => _context.Books.ToList();
        public BookWithAuthorsVM GetBookById(int bookId) 
        {
            var _bookWithAuthors = _context.Books.Where(n => n.Id == bookId).Select(book => new BookWithAuthorsVM()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.DateRead.HasValue ? book.DateRead : null,
                Rate = book.Rate,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                PublisherName = book.Publisher.Name,
                AuthorsNames = book.Book_Authors.Select(n => n.Author.FullName).ToList()
            }).FirstOrDefault();
            return _bookWithAuthors;
        }

        public Book UpdateBookById(int bookId, BookVM book) {
            var _book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (_book != null) {

                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.DateRead.HasValue ? book.DateRead : null;
                _book.Rate = book.Rate;
                _book.Genre = book.Genre;
                _book.CoverUrl = book.CoverUrl;
                _context.SaveChanges();
            }
            return _book;
        }

        public void DeleteBookById(int bookId) {
            var _book = _context.Books.FirstOrDefault(b => b.Id == bookId);
            if (_book != null) {
                _context.Books.Remove(_book);
                _context.SaveChanges();
            }
        }
    }
}

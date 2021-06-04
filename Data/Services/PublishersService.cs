using my_books.Data.Models;
using my_books.Data.Paging;
using my_books.Data.ViewModels;
using my_books.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace my_books.Data.Services
{
    public class PublishersService
    {
        private AppDbContext _context;
        public PublishersService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public List<Publisher> GetAllPublishers(string sortBy, string searchString, int? pageNr)
        {
            var allPulishers = _context.Publishers.OrderBy(n => n.Name).ToList();
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy)
                {
                    case "name_desc":
                        allPulishers = allPulishers.OrderByDescending(n => n.Name).ToList();
                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                allPulishers = allPulishers.Where(n => n.Name.Contains(searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();
            }

            //Paging
            int pageSize = 5;
            allPulishers = PaginatedList<Publisher>.Create(allPulishers.AsQueryable(), pageNr ?? 1, pageSize);
            return allPulishers;
        }

        public Publisher AddPublisher(PublisherVM publisherVM)
        {
            if (StringStartsWithNumber(publisherVM.Name))
            {
                throw new PublisherNameException("Name starts with number", publisherVM.Name);
            }

            var _publisher = new Publisher()
            {
                Name = publisherVM.Name,

            };
            _context.Publishers.Add(_publisher);
            _context.SaveChanges();
            return _publisher;
        }

        public Publisher GetPublisherById(int id)
        {
            return _context.Publishers.FirstOrDefault(n => n.Id == id);
        }

        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId)
        {
            var _publisherData = _context.Publishers.Where(n => n.Id == publisherId)
                    .Select(n => new PublisherWithBooksAndAuthorsVM()
                    {
                        Name = n.Name,
                        BookAuthors = n.Books.Select(n => new BookAuthorVM()
                        {
                            BookName = n.Title,
                            BookAuthors = n.Book_Authors.Select(n => n.Author.FullName).ToList()
                        }).ToList()
                    }).FirstOrDefault();
            return _publisherData;
        }

        public void DeletePublisherById(int id)
        {
            var _publisher = _context.Publishers.FirstOrDefault(n => n.Id == id);
            if (_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"The publishers with id {id} does not exist");
            }
        }

        private bool StringStartsWithNumber(string name) => (Regex.IsMatch(name, @"^\d"));

    }
}

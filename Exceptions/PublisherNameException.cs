using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace my_books.Exceptions
{
    public class PublisherNameException:Exception
    {
        public string PublisherName { get; set; }

        public PublisherNameException()
        {

        }

        public PublisherNameException(string message):base(message)
        {

        }

        public PublisherNameException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public PublisherNameException(string message, string publisherName) : this(message)
        {
            PublisherName = publisherName;
        }
    }
}

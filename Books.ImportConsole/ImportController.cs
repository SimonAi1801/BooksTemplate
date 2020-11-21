using Books.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Books.ImportConsole
{
    public static class ImportController
    {
        private const string FileName = "books.csv";
        private const char Seperator = '~';
        public static IEnumerable<Book> ReadBooksFromCsv()
        {
            string[][] matrix = MyFile.ReadStringMatrixFromCsv(FileName, false);
            IList<Book> books = new List<Book>();
            IList<BookAuthor> bookAuthors = new List<BookAuthor>();
            IDictionary<string, Author> authors = new Dictionary<string, Author>();

            foreach (var line in matrix)
            {
                Book book = new Book
                {
                    Title = line[1],
                    Publishers = line[2],
                    Isbn = line[3]
                };

                string[] names = line[0].Split(Seperator);
                foreach (var name in names)
                {
                    Author author;
                    if (!authors.TryGetValue(name, out author))
                    {
                        author = new Author
                        {
                            Name = name
                        };
                        authors.Add(name, author);
                    }

                    BookAuthor bookAuthor = new BookAuthor
                    {
                        Author = author,
                        Book = book
                    };
                    book.BookAuthors.Add(bookAuthor);
                }
                books.Add(book);
            }
            return books;
        }
    }
}

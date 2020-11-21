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
        public static IEnumerable<Book> ReadBooksFromCsv()
        {
            string[][] matrix = MyFile.ReadStringMatrixFromCsv(FileName, false);

            var authors = matrix.GroupBy(line => line[0])
                                .Select(grp => new Author()
                                {
                                    Name = grp.Key
                                })
                                .ToArray();

            var books = matrix.GroupBy(line => line[1] + ';' +
                                               line[2] + ';' +
                                               line[3])
                               .Select(grp => new Book()
                               {
                                   Title = grp.Key.Split(';')[0],
                                   Publishers = grp.Key.Split(';')[1],
                                   Isbn = grp.Key.Split(';')[2]
                               })
                               .ToArray();

            var bookAutors = matrix.GroupBy(line => line[0] + ';' +
                                                    line[3])
                                   .Select(grp => new BookAuthor()
                                   {
                                       Author = authors.SingleOrDefault(a => a.Name.Equals(grp.Key.Split(';')[0])),
                                       Book = books.SingleOrDefault(b => b.Isbn.Equals(grp.Key.Split(';')[1]))
                                   })
                                   .ToArray();

            foreach (var author in authors)
            {
                author.BookAuthors = bookAutors.Where(ba => ba.Author.Name.Equals(author.Name)).ToList();
            }

            foreach (var book in books)
            {
                book.BookAuthors = bookAutors.Where(ba => ba.Book.Isbn.Equals(book.Isbn)).ToList();
            }
            return books;
        }
    }
}

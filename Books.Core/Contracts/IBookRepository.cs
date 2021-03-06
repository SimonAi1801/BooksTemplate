﻿using Books.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.Core.Contracts
{
    public interface IBookRepository
    {
        Task AddRangeAsync(IEnumerable<Book> books);
        Task<Book[]> GetAllBooksAsync();
        Task<Book[]> GetBooksByFilterAsync(string filterText);
        void DeleteBook(Book book);
        Task<string[]> GetAllPublishersAsync();
        Task<Book> GetBookByIdAsync(int id);
    }
}
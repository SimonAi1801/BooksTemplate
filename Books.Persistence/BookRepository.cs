using Books.Core.Contracts;
using Books.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Persistence
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddRangeAsync(IEnumerable<Book> books)
        => await _dbContext.AddRangeAsync(books);

        public async Task<Book[]> GetAllBooksAsync()
        => await _dbContext.Books
                           .Include(_ => _.BookAuthors)
                           .ThenInclude(_ => _.Author)
                           .OrderBy(_ => _.Title)
                           .ToArrayAsync();

        public async Task<Book[]> GetBooksByFilter(string filterText)
        => await _dbContext.Books
                           .Where(b => EF.Functions.Like(b.Title, $"%{filterText}%"))
                           .Include(b => b.BookAuthors)
                           .ThenInclude(a => a.Author)
                           .OrderBy(b => b.Title)
                           .ToArrayAsync();
    }

}
using Books.Core.Contracts;
using Books.Core.Entities;
using System;
using System.Collections.Generic;
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

    public Task AddRangeAsync(IEnumerable<Book> books)
    {
      throw new NotImplementedException();
    }
  }

}
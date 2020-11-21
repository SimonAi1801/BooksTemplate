using Books.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Books.Core.Contracts
{
    public interface IBookRepository
    {
        Task AddRangeAsync(IEnumerable<Book> books);

        Task<Book[]> GetAllBooksAsync();

        Task<Book[]> GetBooksByFilter(string filterText);
    }
}
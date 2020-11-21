using Books.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Books.Core.DataTransferObjects;

namespace Books.Core.Contracts
{
    public interface IAuthorRepository
    {
        Task<Author[]> GetAllAuthorsAsync();
    }
}
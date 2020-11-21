using Books.Core.Contracts;
using Books.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Books.Core.DataTransferObjects;
using Microsoft.EntityFrameworkCore;

namespace Books.Persistence
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
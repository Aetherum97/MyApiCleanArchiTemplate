using Microsoft.EntityFrameworkCore;
using MyApiTemplateCleanArchi.Domain.Entities;
using MyApiTemplateCleanArchi.Domain.Interfaces;
using MyApiTemplateCleanArchi.Infrastructure.Commons.Bases;
using MyApiTemplateCleanArchi.Shared.Commons.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiTemplateCleanArchi.Infrastructure.Persistence.Repositories
{
    public class UserRepository : BaseRepository<User, ApplicationDbContext>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username) ?? throw new Exception("User not found");
        }

        public async Task<PagedList<User>> GetAllUsersAsync(PaginationParameters parameters)
        {
            var query = _dbSet.AsQueryable();
            return await PagedList<User>.CreateAsync(query, parameters.PageNumber, parameters.PageSize);
        }
    }
}

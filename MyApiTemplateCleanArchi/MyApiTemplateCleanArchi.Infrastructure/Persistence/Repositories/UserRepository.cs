using Microsoft.EntityFrameworkCore;
using MyApiTemplateCleanArchi.Domain.Entities;
using MyApiTemplateCleanArchi.Domain.Interfaces;
using MyApiTemplateCleanArchi.Shared.Commons.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiTemplateCleanArchi.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task AddAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }


        public async Task<User> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        }


        public async Task<PagedList<User>> GetAllUsersAsync(PaginationParameters parameters)
        {
            var query = _dbContext.Users.AsQueryable();
            return await PagedList<User>.CreateAsync(query, parameters.PageNumber, parameters.PageSize);
        }
    }
}

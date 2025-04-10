using MyApiTemplateCleanArchi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyApiTemplateCleanArchi.Shared.Commons.Pagination;

namespace MyApiTemplateCleanArchi.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<PagedList<User>> GetAllUsersAsync(PaginationParameters parameters);
        Task AddAsync(User user);
    }
}

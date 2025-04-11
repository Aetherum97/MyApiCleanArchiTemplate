using MyApiTemplateCleanArchi.Application.DTOs;
using MyApiTemplateCleanArchi.Application.Modules.Interfaces;
using MyApiTemplateCleanArchi.Shared.Commons.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiTemplateCleanArchi.Application.Queries.GetAllUsers
{
    public record GetAllUsersQuery(PaginationParameters Parameters) : IRequest<PagedList<UserDto>>;

}

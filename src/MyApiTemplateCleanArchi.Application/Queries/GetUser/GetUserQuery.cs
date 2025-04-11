using MyApiTemplateCleanArchi.Application.DTOs;
using MyApiTemplateCleanArchi.Application.Modules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiTemplateCleanArchi.Application.Queries.GetUser
{
    public record GetUserQuery(int Id) : IRequest<UserDto>;
}

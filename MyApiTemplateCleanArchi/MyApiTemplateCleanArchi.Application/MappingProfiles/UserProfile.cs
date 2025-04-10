using AutoMapper;
using MyApiTemplateCleanArchi.Application.DTOs;
using MyApiTemplateCleanArchi.Domain.Entities;
using MyApiTemplateCleanArchi.Shared.Commons.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyApiTemplateCleanArchi.Application.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User,UserDto>();

            CreateMap(typeof(PagedList<>), typeof(PagedList<>))
                .ConvertUsing(typeof(PagedListConverter<,>));

        }
    }
}

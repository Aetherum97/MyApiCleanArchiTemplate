using MyApiTemplateCleanArchi.Domain.Entities;
using MyApiTemplateCleanArchi.Domain.Interfaces;
using MyApiTemplateCleanArchi.Infrastructure.Commons.Bases;
using MyApiTemplateCleanArchi.Infrastructure.Persistence.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiTemplateCleanArchi.Infrastructure.Persistence.Repositories
{
    public class TodoRepository : BaseRepository<Todo, TodoPostgreDbContext>, ITodoRepository
    {
        public TodoRepository(TodoPostgreDbContext context)
            : base(context)
        {
        }
    }
}

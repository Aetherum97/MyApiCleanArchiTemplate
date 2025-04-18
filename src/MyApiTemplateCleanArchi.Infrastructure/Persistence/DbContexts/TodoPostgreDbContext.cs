using Microsoft.EntityFrameworkCore;
using MyApiTemplateCleanArchi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiTemplateCleanArchi.Infrastructure.Persistence.DbContexts
{
    public class TodoPostgreDbContext : DbContext
    {
        public TodoPostgreDbContext(DbContextOptions<TodoPostgreDbContext> options)
            : base(options)
        {
        }
        public DbSet<Todo> Todos { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MyApiTemplateCleanArchi.Domain.Entities.Todo>().HasData(new MyApiTemplateCleanArchi.Domain.Entities.Todo
            {
                Id = 1,
                Title = "Todo 1",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            });
        }
    }
}

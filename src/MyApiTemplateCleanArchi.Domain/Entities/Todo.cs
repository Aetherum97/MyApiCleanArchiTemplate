using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiTemplateCleanArchi.Domain.Entities
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

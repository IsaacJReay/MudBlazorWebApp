using Microsoft.EntityFrameworkCore;

namespace mptc.dgc.isaac.verify.dal.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }
        public DbSet<Todo> Todos { get; set; } = default!;
    }
}
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data
{
    public class TodoApiContext : DbContext
    {
        public TodoApiContext(DbContextOptions<TodoApiContext> options) : base(options) { }

        public virtual DbSet<TodoItem> TodoItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .Property(b => b.Created)
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");
        }
    }
}

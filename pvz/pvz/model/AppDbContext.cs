using Microsoft.EntityFrameworkCore;
using System.IO;

namespace mvvmsample.model
{
    public class AppDbContext : DbContext
    {
        public DbSet<ProductModel> Orders { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string dbPath = Path.GetFullPath(Path.Combine("..", "..", "..", "..", "..", "..", "BARSUK", "database", "barsukDB.db"));
            optionsBuilder.UseSqlite($"Data Source={dbPath};");
        }
    }
}

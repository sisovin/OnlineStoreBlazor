using Microsoft.EntityFrameworkCore;
using OnlineStoreSharedLibrary.Models;

namespace OnlineStoreApiBlazor.Data
{
    public class ApplicationDbContext : DbContext
    { 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; } = default!;        
    }
}

using Microsoft.EntityFrameworkCore;
using VillaApi.Model;

namespace VillaApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Villa> villas { get; set; }
    }
}

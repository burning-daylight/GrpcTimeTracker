using GrpcTimeTrackerServiceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace GrpcTimeTrackerServiceApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ActiveItem> ActiveItems => Set<ActiveItem>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
    }
}

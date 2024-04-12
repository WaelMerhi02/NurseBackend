using Microsoft.EntityFrameworkCore;
using NurseApp.Models;
namespace NurseApp
{
    public class NurseAppDbContext:DbContext
    {
        
        public NurseAppDbContext(DbContextOptions<NurseAppDbContext> options): base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<NurseAdvancedInfo> NurseAdvancedInfo { get; set; }

        public DbSet<NurseAvailableDates> NurseAvailableDates { get; set; }
        public DbSet<NurseDetails> NurseDetails { get; set; }
    }
}

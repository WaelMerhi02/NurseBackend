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
        public DbSet<NurseAdvancedInfo>  NurseAdvancedInfos { get; set; }
    }
}

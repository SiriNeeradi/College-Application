using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CollegeApp.Data
{
    public class CollegeDBContext : DbContext
    {
        public CollegeDBContext(DbContextOptions<CollegeDBContext> options) : base(options)
        {
                
        }

        
        DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Table1
            modelBuilder.ApplyConfiguration(new Config.StudentConfig());
        }

    }
}

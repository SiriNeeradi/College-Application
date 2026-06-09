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
            modelBuilder.Entity<Student>().HasData(new List<Student>()
            {
                new Student
                {
                    Id          = 1,
                    StudentName = "Alice",
                    Address     = "India",
                    Email       = "alice@gmail.com",
                    DOB         = new DateTime(2022, 12, 12)
                },
                new Student
                {
                    Id          = 2,
                    StudentName = "Tom",
                    Address     = "India",
                    Email       = "tom@gmail.com",
                    DOB         = new DateTime(2022, 10, 12)
                }

            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.Property(n => n.StudentName).IsRequired().HasMaxLength(50);
                entity.Property(n => n.Address).IsRequired(false).HasMaxLength(200);
                entity.Property(n => n.Email).IsRequired().HasMaxLength(200);
            });
        }

    }
}

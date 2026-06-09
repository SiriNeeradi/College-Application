using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();


            builder.Property(n => n.StudentName).IsRequired().HasMaxLength(50);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(200);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(200);

            builder.HasData(new List<Student>()
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
        }
    }
}


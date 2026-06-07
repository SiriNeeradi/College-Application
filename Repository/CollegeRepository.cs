using CollegeApp.Models;

namespace CollegeApp.Repository
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>()
            {
                new Student
                {
                    Id = 1,
                    Name = "John",
                    Email = "john@gmail.com",
                    Address = "Hyd, India"
                },

                new Student
                {
                    Id = 2,
                    Name = "Jane",
                    Email = "jane@gmail.com",
                    Address = "Bangalore, India"
                }
        };
    }
}

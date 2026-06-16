using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDBContext _dBContext;


        public StudentRepository(CollegeDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _dBContext.Students.ToListAsync();
        }

        public async Task<Student> GetByIDAsync(int id, bool useNoTracking = false)
        {
            if(useNoTracking) 
                return await _dBContext.Students.AsNoTracking().Where(s => s.Id == id).FirstOrDefaultAsync();
            else
                return await _dBContext.Students.Where(s => s.Id == id).FirstOrDefaultAsync();

        }

        public async Task<Student> GetByNameAsync(string name)
        {
            return await _dBContext.Students.Where(s => s.StudentName.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();
        }
        
        public async Task<int> CreateAsync(Student student)
        {
            _dBContext.Students.Add(student);
            await _dBContext.SaveChangesAsync();
            return student.Id;
        }

        public async Task<int> UpdateAsync(Student student)
        {
            
            _dBContext.Update(student);
            await _dBContext.SaveChangesAsync();

            return student.Id;
          
        }   
        

        public async Task<bool> DeleteAsync(Student student)
        {
           
            _dBContext.Remove(student);
            await _dBContext.SaveChangesAsync();
            return true;
        }

        public Task<Student> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Student> GetByIDAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}


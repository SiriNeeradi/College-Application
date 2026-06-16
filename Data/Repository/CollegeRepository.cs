using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Repository
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        private readonly CollegeDBContext _dBContext;
        private DbSet<T> _dbSet;

        public CollegeRepository(CollegeDBContext dBContext)
        {
            _dBContext = dBContext;
            _dbSet = _dBContext.Set<T>();
        }

        public async Task<T> CreateAsync(T dbRecord)
        {
            _dbSet.Add(dbRecord);
            await _dBContext.SaveChangesAsync();
            return dbRecord;
        }

        public async Task<bool> DeleteAsync(T dbRecord)
        {
            _dbSet.Remove(dbRecord);
            await _dBContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        //public async Task<T> GetByIDAsync(int id, bool useNoTracking = false)
        //{
        //    if(useNoTracking)
        //        return await _dbSet.AsNoTracking().Where(s => s.Id == id).FirstOrDefaultAsync();
        //    else
        //        return await _dbSet.Where(s => s.Id == id).FirstOrDefaultAsync();

        
        //}

        //public Task<T> GetByNameAsync(string name)
        //{
        //    return await _dbSet.Where(s => s.StudentName.ToLower().Contains(name.ToLower())).FirstOrDefaultAsync();

        //}


        public async Task<T> UpdateAsync(T dbRecord)
        {
            _dbSet.Update(dbRecord);
            await _dBContext.SaveChangesAsync();

            return dbRecord;
        }
    }
}

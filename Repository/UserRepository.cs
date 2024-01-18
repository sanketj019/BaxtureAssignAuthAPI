using BaxtureAssignAuthAPI.DBContext;
using BaxtureAssignAuthAPI.Models;
using BaxtureAssignAuthAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaxtureAssignAuthAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _dbContext;
        public UserRepository(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool UserExists(string id)
        {
            return  (_dbContext.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task CreateUser(User user)
        {
            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();     
            
        }

        public async Task DeleteUser(string UserId)
        {
            _dbContext.Remove(UserId);
            await _dbContext.SaveChangesAsync();
            
        }

        public async Task<User> GetUserById(string userId)
        {
            return await _dbContext.User.Where(x => x.Id == userId).FirstOrDefaultAsync();
            
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
           return await _dbContext.User.ToListAsync();
        }

        public async Task<User> UpdateUser(User user)
        {
            _dbContext.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> SearchUsersAsync(UserSearchRequest searchRequest)
        {
            var query = _dbContext.User.AsQueryable();

            if (!string.IsNullOrEmpty(searchRequest.FieldName) && !string.IsNullOrEmpty(searchRequest.FieldValue))
            {
                // Build a dynamic expression to filter based on field name and value
                var parameter = Expression.Parameter(typeof(User), "u");
                var property = Expression.Property(parameter, searchRequest.FieldName);
                var constant = Expression.Constant(searchRequest.FieldValue, typeof(string));
                var equality = Expression.Equal(property, constant);
                var lambda = Expression.Lambda<Func<User, bool>>(equality, parameter);

                // Apply the filter expression
                query = query.Where(lambda);
            }

            if (!string.IsNullOrEmpty(searchRequest.SortBy))
            {
                if (searchRequest.SortDescending)
                {
                    // OrderByDescending
                    query = query.OrderByDescending(u => EF.Property<string>(u, searchRequest.SortBy));
                }
                else
                {
                    // OrderBy
                    query = query.OrderBy(u => EF.Property<string>(u, searchRequest.SortBy));
                }
            }

            return await query.ToListAsync();
        }


    }
}

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StudentBlogg.Data;
using StudentBlogg.Feature.Users.Interfaces;
namespace StudentBlogg.Feature.Users;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly StudentBloggDbContext _dbContext;

    public UserRepository(ILogger<UserRepository> logger, StudentBloggDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetPagedAsync(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        var users = _dbContext.Users
            .OrderBy(u => u.Id)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
        
        _logger.LogInformation($"GetPagedAsync starting at {skip} of {pageSize}");
        return await users;
    }

    public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
    {
        return await _dbContext.Users.Where(predicate).ToListAsync();
    }

    public async Task<User?> AddAsync(User entity)
    {
        await _dbContext.Users.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        return entity;
    }

    public Task<User?> UpdateByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> DeleteByIdAsync(Guid id)
    {
        var user = await _dbContext.Users.FindAsync(id);
        await _dbContext.Users.Where(u => u.Id == id).ExecuteDeleteAsync();
        
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public Task<User?> UpdateByIdAsync(User entity)
    {
        throw new NotImplementedException();
    }
    
}
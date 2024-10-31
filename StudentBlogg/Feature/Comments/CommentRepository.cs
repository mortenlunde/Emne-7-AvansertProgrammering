using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using StudentBlogg.Data;
using StudentBlogg.Feature.Comments.Interfaces;

namespace StudentBlogg.Feature.Comments;

public class CommentRepository : ICommentRepository
{
    private readonly ILogger<CommentRepository> _logger;
    private readonly StudentBloggDbContext _dbContext;

    public CommentRepository(ILogger<CommentRepository> logger, StudentBloggDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    public async Task<Comment?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Comment>> GetPagedAsync(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;

        var comments = _dbContext.Comments
            .OrderBy(c => c.DateCommented)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
        
        _logger.LogInformation($"GetPagedAsync started for {pageSize} of {pageSize}.");
        
        return await comments;
    }

    public async Task<IEnumerable<Comment>> FindAsync(Expression<Func<Comment, bool>> predicate)
    {
        return await _dbContext.Comments.Where(predicate).ToListAsync();
    }

    public async Task<Comment?> AddAsync(Comment entity)
    {
        await _dbContext.Comments.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        
        return entity;
    }

    public async Task<Comment?> UpdateByIdAsync(Guid id, Comment entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Comment?> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
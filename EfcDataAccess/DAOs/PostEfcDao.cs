using Application.DaoInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared;
using Shared.DTOs;

namespace EfcDataAccess.DAOs;

public class PostEfcDao : IPostDao
{
    private readonly PostContext context;

    public PostEfcDao(PostContext context)
    {
        this.context = context;
    }
    
    public async Task<Post> CreateAsync(Post post)
    {
        EntityEntry<Post> added = await context.Posts.AddAsync(post);
        await context.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters)
    {
        IQueryable<Post> query = context.Posts.Include(post => post.Owner).AsQueryable();

        if (!string.IsNullOrEmpty(searchParameters.Username))
        {
            query = query.Where(post =>
                post.Owner.UserName.ToLower().Equals(searchParameters.Username.ToLower()));
        }

        if (!string.IsNullOrEmpty(searchParameters.TitleContains))
        {
            query = query.Where(t =>
                t.Title.ToLower().Contains(searchParameters.TitleContains.ToLower()));
        }

        if (searchParameters.UserId != null)
        {
            query = query.Where(t => t.Owner.Id == searchParameters.UserId);
        }
        
        List<Post> result = await query.ToListAsync();
        return result;
    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        Post? found = await context.Posts
            .AsNoTracking()
            .Include(todo => todo.Owner)
            .SingleOrDefaultAsync(todo => todo.Id == id);
        return found;
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await GetByIdAsync(id);
        if (existing == null)
        {
            throw new Exception($"Post with id {id} not found");
        }

        context.Posts.Remove(existing);
        await context.SaveChangesAsync();
    }
}
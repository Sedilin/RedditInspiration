using Application.DaoInterfaces;
using Shared;
using Shared.DTOs;

namespace FileData.DAOs;

public class PostFileDao : IPostDao
{
    private readonly FileContext context;

    public PostFileDao(FileContext context)
    {
        this.context = context;
    }

    public Task<Post> CreateAsync(Post post)
    {
        int postId = 0;
        if (context.Posts.Any())
        {
            postId = context.Posts.Max(p => p.Id);
            postId++;
        }

        post.Id = postId;
        
        context.Posts.Add(post);
        context.SaveChanges();
        
        return Task.FromResult(post);
    }

    public Task<IEnumerable<Post>> GetAsync(SearchPostParametersDto searchParameters)
    {
        IEnumerable<Post> result = context.Posts.AsEnumerable();

        if (!string.IsNullOrEmpty(searchParameters.Username))
        {
            result = context.Posts.Where(post =>
                post.Owner.UserName.Equals(searchParameters.Username, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(searchParameters.TitleContains))
        {
            result = result.Where(t =>
                t.Title.Contains(searchParameters.TitleContains, StringComparison.OrdinalIgnoreCase));
        }

        if (searchParameters.UserId != null)
        {
            result = result.Where(t => t.Owner.Id == searchParameters.UserId);
        }

        return Task.FromResult(result);
    }

    public Task<Post?> GetByIdAsync(int id)
    {
        Post? existing = context.Posts.FirstOrDefault(p => p.Id == id);
        return Task.FromResult(existing);
    }

    public Task DeleteAsync(int id)
    {
        Post? existing = context.Posts.FirstOrDefault(p => p.Id == id);
        if (existing == null)
        {
            throw new Exception($"Todo with id {id} does not exist");
        }

        context.Posts.Remove(existing);
        context.SaveChanges();
        
        return Task.CompletedTask;
    }
}
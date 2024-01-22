using AlphaForums.Data;
using AlphaForums.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AlphaForums.Service;

public class PostService : IPost
{
    private readonly ApplicationDbContext _context;

    public PostService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Post GetById(int id)
    {
        return _context.Posts
            .Where(post => post.Id == id)
            .Include(post => post.Forum)
            .Include(post => post.User)
            .Include(post => post.Relies)
            .ThenInclude(rely => rely.User)
            .First();
    }

    public IEnumerable<Post> GetAll()
    {
        return _context.Posts
            .Include(post => post.Forum)
            .Include(post => post.User)
            .Include(post => post.Relies)
            .ThenInclude(rely => rely.User);
    }

    public IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery)
    {
        return string.IsNullOrEmpty(searchQuery)
            ? forum.Posts
            : forum.Posts.Where(post
                => post.Title.ToLower().Contains(searchQuery.ToLower())
                   || post.Content.ToLower().Contains(searchQuery.ToLower())
            );
    }

    public IEnumerable<Post> GetFilteredPosts(string searchQuery)
    {
        return string.IsNullOrEmpty(searchQuery)
            ? new List<Post>()
            : GetAll().Where(post
                => post.Title.ToLower().Contains(searchQuery.ToLower())
                   || post.Content.ToLower().Contains(searchQuery.ToLower())
            );
    }

    public IEnumerable<Post> GetLastPosts(int num)
    {
        return GetAll().OrderByDescending(post => post.Created).Take(num);
    }

    public async Task Add(Post post)
    {
        _context.Add(post);
        await _context.SaveChangesAsync();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task EditPostContent(int id, string newContent)
    {
        throw new NotImplementedException();
    }

    public Task AddRely(PostReply reply)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Post> GetPostsByForum(int id)
    {
        var posts = _context.Forums
            .Where(forum => forum.Id == id)
            .Include(forum => forum.Posts)
            .First()
            .Posts;
        return posts;
    }
}
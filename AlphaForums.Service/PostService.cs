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
            .Include(post => post.Forum)
            .Include(post => post.User)
            .Include(post => post.Relies)
            .ThenInclude(rely => rely.User)
            .First(post => post.Id == id && post.Enable == true && post.Forum.Enable == true);
    }

    public IEnumerable<Post> GetAll()
    {
        return _context.Posts
            .Include(post => post.Forum)
            .Include(post => post.User)
            .Include(post => post.Relies)
            .ThenInclude(rely => rely.User)
            .Where(p => p.Enable == true && p.Forum.Enable == true);
    }

    public IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery)
    {
        return string.IsNullOrEmpty(searchQuery)
            ? GetPostsByForum(forum)
            : GetPostsByForum(forum).Where(post
                => post.Title.ToLower().Contains(searchQuery.ToLower())
                   || post.Content.ToLower().Contains(searchQuery.ToLower())
            );
    }

    public IEnumerable<Post> GetPostsByForum(Forum forum)
    {
        return forum.Posts.Where(p => p.Enable == true);
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

    public async Task Delete(int id)
    {
        var post = _context.Posts.FirstOrDefault(p => p.Id == id);
        if(post == null) return;
        post.Enable = false;
        await _context.SaveChangesAsync();
    }

    public async Task EditPost(int id, string newTitle, string newContent)
    {
        var post = GetById(id);
        if (post == null) return;
        post.Title = newTitle;
        post.Content = newContent;
        await _context.SaveChangesAsync();
    }

    public async Task AddReply(PostReply reply)
    {
        _context.PostRelies.Add(reply);
        await _context.SaveChangesAsync();
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
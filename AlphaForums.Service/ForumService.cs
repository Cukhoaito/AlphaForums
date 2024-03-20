using AlphaForums.Data;
using AlphaForums.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AlphaForums.Service;

public class ForumService : IForum
{
    private readonly ApplicationDbContext _context;

    public ForumService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Forum GetById(int id)
    {
        var forum = _context.Forums
            .Where(forum => forum.Id == id)
            .Include(forum => forum.Posts)
            .ThenInclude(post => post.User)
            .Include(forum => forum.Posts)
            .ThenInclude(post => post.Relies)
            .ThenInclude(reply => reply.User)
            .First();
        return forum;
    }

    public IEnumerable<Forum> GetAll()
    {
        return _context.Forums
            .Include(forum => forum.Posts);
    }

    

    public async Task Create(Forum forum)
    {
        _context.Forums.Add(forum);
        await _context.SaveChangesAsync();
    }

    public Task Delete(int forumId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateForumImageUrl(int forumId, string url)
    {
        var forum = _context.Forums.First(f => f.Id == forumId);
        if (forum == null) return;
        forum.ImageUrl = url;
        await _context.SaveChangesAsync();
    }

    public Task UpdateForumTitle(int forumId, string newTitle)
    {
        throw new NotImplementedException();
    }

    public Task UpdateForumDescription(int forumId, string newDescription)
    {
        throw new NotImplementedException();
    }

    public bool HasRecentPost(int forumId)
    {
        // const int hoursAgo = 12;
        // var window = DateTime.Now.AddHours(-hoursAgo);
        // return GetById(forumId).Posts.Any(post => post.Created > window);
        return false;
    }
}
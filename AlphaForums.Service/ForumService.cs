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
            .Where(forum => forum.Id == id && forum.Enable == true)
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
            .Include(forum => forum.Posts).Where(f => f.Enable == true);
    }

    

    public async Task Create(Forum forum)
    {
        _context.Forums.Add(forum);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int forumId)
    {
        var forum = GetById(forumId);
        if (forum==null) return;
        forum.Enable = false;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateForum(int forumId, string title, string description)
    {
        var forum = _context.Forums.First(f => f.Id == forumId && f.Enable == true);
        if (forum == null) return;
        forum.Title = title;
        forum.Description = description;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateForumImageUrl(int forumId, string url)
    {
        var forum = _context.Forums.First(f => f.Id == forumId && f.Enable == true);
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
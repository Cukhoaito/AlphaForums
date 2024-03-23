using AlphaForums.Data.Models;

namespace AlphaForums.Data;

public interface IForum
{
    Forum GetById(int id);
    IEnumerable<Forum> GetAll();

    Task Create(Forum forum);
    Task Delete(int forumId);
    Task UpdateForum(int forumId, string title, string description);

    Task UpdateForumImageUrl(int forumId, string url);
    Task UpdateForumTitle(int forumId, string newTitle);
    Task UpdateForumDescription(int forumId, string newDescription);

    bool HasRecentPost(int forumId); 
}
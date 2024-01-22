using AlphaForums.Data.Models;

namespace AlphaForums.Data;

public interface IForum
{
    Forum GetById(int id);
    IEnumerable<Forum> GetAll();
    IEnumerable<ApplicationUser> GetAllActiveUsers();

    Task Create(Forum forum);
    Task Delete(int forumId);
    Task UpdateForumTitle(int forumId, string newTitle);
    Task UpdateForumDescription(int forumId, string newDescription);

}
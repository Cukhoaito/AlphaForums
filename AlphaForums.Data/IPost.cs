using AlphaForums.Data.Models;

namespace AlphaForums.Data;

public interface IPost
{
    Post GetById(int id);
    IEnumerable<Post> GetAll();
    IEnumerable<Post> GetFilteredPosts(Forum forum,string searchQuery);
    IEnumerable<Post> GetFilteredPosts(string searchQuery);

    IEnumerable<Post> GetLastPosts(int num); 

    Task Add(Post post);
    Task Delete(int id);
    Task EditPostContent(int id, string newContent);

    Task AddRely(PostReply reply);
    IEnumerable<Post> GetPostsByForum(int id);
}
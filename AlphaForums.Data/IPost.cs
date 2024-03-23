using AlphaForums.Data.Models;

namespace AlphaForums.Data;

public interface IPost
{
    Post GetById(int id);
    IEnumerable<Post> GetAll();
    IEnumerable<Post> GetFilteredPosts(Forum forum,string searchQuery);
    IEnumerable<Post> GetPostsByForum(Forum forum);

    IEnumerable<Post> GetFilteredPosts(string searchQuery);

    IEnumerable<Post> GetLastPosts(int num); 

    Task Add(Post post);
    Task Delete(int id);
    Task EditPost(int id, string newTitle, string newContent);

    Task AddReply(PostReply reply);
    IEnumerable<Post> GetPostsByForum(int id);
}
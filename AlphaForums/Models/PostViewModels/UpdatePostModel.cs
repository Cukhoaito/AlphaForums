namespace AlphaForums.Models.PostViewModels;

public class UpdatePostModel
{
    public int PostId { get; set; }
    public string ForumName { get; set; }
    public int ForumId { get; set; } 
    public string AuthorName { get; set; }
    public string ForumImageUrl { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
}
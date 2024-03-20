namespace AlphaForums.Models.ForumViewModels;

public class AddForumViewModel
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public IFormFile File { get; set; }
}
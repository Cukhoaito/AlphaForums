namespace AlphaForums.Models.ForumViewModels;

public class UpdateForumViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public IFormFile File { get; set; }
}
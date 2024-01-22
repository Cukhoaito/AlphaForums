namespace AlphaForums.Models.PostViewModels;

public class PostRelyModel
{
    public int Id { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int AuthorRating { get; set; }
    public string AuthorImageUrl { get; set; }
    public DateTime Created { get; set; }
    public string RelyContent { get; set; }
    
    public int PostId { get; set; }
    public bool IsAuthorAdmin { get; set; }
}
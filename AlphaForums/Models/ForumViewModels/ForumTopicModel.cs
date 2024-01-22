using AlphaForums.Models.PostViewModels;

namespace AlphaForums.Models.ForumViewModels;

public class ForumTopicModel
{ 
    public ForumListingModel Forum { get; set; }
    public IEnumerable<PostListingModel> Posts { get; set; }
}
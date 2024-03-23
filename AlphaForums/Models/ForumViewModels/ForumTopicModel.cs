using AlphaForums.Models.PostViewModels;
using X.PagedList;

namespace AlphaForums.Models.ForumViewModels;

public class ForumTopicModel
{ 
    public ForumListingModel Forum { get; set; }
    public IPagedList<PostListingModel> Posts { get; set; }
    public string SearchQuery { get; set; }
}
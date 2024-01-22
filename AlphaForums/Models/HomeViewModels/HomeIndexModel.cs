using AlphaForums.Data.Models;
using AlphaForums.Models.PostViewModels;

namespace AlphaForums.Models.HomeViewModels;

public class HomeIndexModel
{
    public IEnumerable<PostListingModel> LastPosts { get; set; }
    
}
using AlphaForums.Models.PostViewModels;

namespace AlphaForums.Models.SearchViewModels;

public class SearchResultModel
{
    public IEnumerable<PostListingModel> Posts { get; set; }
    public string SearchQuery { get; set; }
}
using System.Globalization;
using AlphaForums.Data;
using AlphaForums.Data.Models;
using AlphaForums.Models.ForumViewModels;
using AlphaForums.Models.PostViewModels;
using AlphaForums.Models.SearchViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlphaForums.Controllers;

public class SearchController : Controller
{
    private readonly IPost _postService;

    public SearchController(IPost postService)
    {
        _postService = postService;
    }
    [HttpGet]
    public IActionResult Results(string query)
    {
        var posts = _postService.GetFilteredPosts(query);
        var postListings = posts.Select(post => new PostListingModel
        {
            Id = post.Id,
            AuthorId = post.User.UserName,
            AuthorName = post.User.UserName,
            AuthorRating = post.User.Rating,
            Title = post.Title,
            DatePosted = post.Created.ToString(CultureInfo.CurrentCulture),
            RepliesCount = post.Relies.Count(),
            Forum = BuildForumListing(post)
        });

        var model = new SearchResultModel
        {
            Posts = postListings,
            SearchQuery = query
        };
        return View(model);
    }
    
    private ForumListingModel BuildForumListing(Post post)
    {
        var forum = post.Forum;

        return BuildForumListing(forum);
    }

    private ForumListingModel BuildForumListing(Forum forum)
    {
        return new ForumListingModel
        {
            Id = forum.Id,
            Name = forum.Title,
            Description = forum.Description,
            ImageUrl = forum.ImageUrl
        };
    }
    
    [HttpPost]
    public IActionResult Search(string query)
    {
        return RedirectToAction("Results", new { query });
    }
}
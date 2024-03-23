using System.Globalization;
using AlphaForums.Data;
using AlphaForums.Data.Models;
using AlphaForums.Models.ForumViewModels;
using AlphaForums.Models.PostViewModels;
using AlphaForums.Models.SearchViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AlphaForums.Controllers;

public class SearchController : Controller
{
    private readonly IPost _postService;

    public SearchController(IPost postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public IActionResult Results(string query, string sort)
    {
        var posts = _postService.GetFilteredPosts(query);
        if (!sort.IsNullOrEmpty())
        {
            posts = sort switch
            {
                "date_inc" => posts.OrderBy(p => p.Created),
                "date_desc" => posts.OrderByDescending(p => p.Created),
                "replies_inc" => posts.OrderBy(p => p.Relies.Count()),
                "replies_desc" => posts.OrderByDescending(p => p.Relies.Count()),
                _ => posts
            };
            
        }

        ViewBag.sort = sort;

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
    public IActionResult Search(string query, string sort)
    {
        return RedirectToAction("Results", new { query, sort });
    }
}
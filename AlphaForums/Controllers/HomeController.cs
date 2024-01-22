using System.Diagnostics;
using System.Globalization;
using AlphaForums.Data;
using AlphaForums.Data.Models;
using Microsoft.AspNetCore.Mvc;
using AlphaForums.Models;
using AlphaForums.Models.ForumViewModels;
using AlphaForums.Models.HomeViewModels;
using AlphaForums.Models.PostViewModels;

namespace AlphaForums.Controllers;

public class HomeController : Controller
{
    private readonly IPost _postService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, IPost postService)
    {
        _logger = logger;
        _postService = postService;
        
    }

    public IActionResult Index()
    {
        var model = BuilderHomeIndexModel();
        return View(model);
    }

    private HomeIndexModel BuilderHomeIndexModel()
    {
        var lastPosts = _postService.GetLastPosts(10);
        var posts = lastPosts.Select(post => new PostListingModel
        {
            Id = post.Id,
            Title = post.Title,
            AuthorId = post.User.Id,
            AuthorName = post.User.UserName,
            AuthorRating = post.User.Rating,
            DatePosted = post.Created.ToString(CultureInfo.InvariantCulture),
            RepliesCount = post.Relies.Count(),
            Forum = GetForumListingForPost(post)
        });
        return new HomeIndexModel
        {
            LastPosts = posts
        };
    }
    
    [HttpPost]
    private IActionResult Search(string query)
    {
        
        return RedirectToAction("Index");
    }

    private ForumListingModel GetForumListingForPost(Post post)
    {
        var forum = post.Forum;
        return new ForumListingModel
        {
            Name = forum.Title,
            ImageUrl = forum.ImageUrl,
            Id = forum.Id
        };
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
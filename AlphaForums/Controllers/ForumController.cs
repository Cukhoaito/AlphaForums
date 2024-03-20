using System.Globalization;
using System.Text.RegularExpressions;
using AlphaForums.Data;
using AlphaForums.Data.Models;
using AlphaForums.Models.ForumViewModels;
using AlphaForums.Models.PostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlphaForums.Controllers;

public class ForumController : Controller
{
    private readonly IForum _forumService;
    private readonly IPost _postService;
    private readonly IWebHostEnvironment _webHost;


    public ForumController(IForum forumService, IPost postService, IWebHostEnvironment webHost)
    {
        _forumService = forumService;
        _postService = postService;
        _webHost = webHost;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var forums = _forumService
            .GetAll()
            .Select(BuildForumListing);

        var model = new ForumIndexModel
        {
            ForumList = forums.OrderBy(f => f.Name)
        };

        return View(model);
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        var model = new AddForumViewModel();
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddForum(AddForumViewModel model)
    {
        var uploadsFolder = Path.Combine(_webHost.WebRootPath, "images", "forum");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }


        var fileName = Path.GetFileName("forum_" + Regex.Replace(model.Title, @"\s+", "") + ".png");
        var fileSavePath = Path.Combine(uploadsFolder, fileName);
        await using (var stream = new FileStream(fileSavePath, FileMode.Create))
        {
            await model.File.CopyToAsync(stream);
            await _forumService.Create(new Forum
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = $"/images/forum/{fileName}"
            });
        }

        return RedirectToAction("Index", "Forum");
    }


    [HttpGet]
    public IActionResult Topic(int id, string query)
    {
        var forum = _forumService.GetById(id);

        var postListings = GetPostListingsForForum(forum, query);

        var model = new ForumTopicModel
        {
            Posts = postListings,
            Forum = BuildForumListing(forum),
            SearchQuery = query
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Search(int id, string query)
    {
        return RedirectToAction("Topic", new { id, query });
    }

    private IEnumerable<PostListingModel> GetPostListingsForForum(Forum forum, string query)
    {
        var posts = string.IsNullOrEmpty(query) ? forum.Posts : _postService.GetFilteredPosts(forum, query);


        return posts.Select(post => new PostListingModel
        {
            Id = post.Id,
            AuthorId = post.User.Id,
            AuthorName = post.User.UserName,
            AuthorRating = post.User.Rating,
            Title = post.Title,
            DatePosted = post.Created.ToString(CultureInfo.CurrentCulture),
            RepliesCount = post.Relies.Count(),
            Forum = BuildForumListing(post)
        });
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
            ImageUrl = forum.ImageUrl,
            NumberOfPosts = forum.Posts?.Count() ?? 0,
            HasRecentPost = _forumService.HasRecentPost(forum.Id)
        };
    }
}
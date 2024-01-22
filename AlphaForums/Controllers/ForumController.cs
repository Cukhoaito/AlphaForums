using System.Globalization;
using AlphaForums.Data;
using AlphaForums.Data.Models;
using AlphaForums.Models.ForumViewModels;
using AlphaForums.Models.PostViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AlphaForums.Controllers;

public class ForumController : Controller
{
    private readonly IForum _forumService;
    private readonly IPost _postService;


    public ForumController(IForum forumService, IPost postService)
    {
        _forumService = forumService;
        _postService = postService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var forums = _forumService
            .GetAll()
            .Select(BuildForumListing);

        var model = new ForumIndexModel
        {
            ForumList = forums
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Topic(int id, string query)
    {
        var forum = _forumService.GetById(id);

        var postListings = GetPostListingsForForum(forum, query);

        var model = new ForumTopicModel
        {
            Posts = postListings,
            Forum = BuildForumListing(forum)
        };

        return View(model);
    }

    public IActionResult Create()
    {
        throw new NotImplementedException();
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
            ImageUrl = forum.ImageUrl
        };
    }
}
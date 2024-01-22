using AlphaForums.Data;
using AlphaForums.Data.Models;
using AlphaForums.Models.PostViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlphaForums.Controllers;

public class PostController : Controller
{
    private readonly IPost _postService;
    private readonly IForum _forumService;
    private readonly UserManager<ApplicationUser> _userManager;

    public PostController(IPost postService, IForum forumService, UserManager<ApplicationUser> userManager)
    {
        _postService = postService;
        _forumService = forumService;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Index(int id)
    {
        var post = _postService.GetById(id);
        var replies = BuildPostReplies(post.Relies);
        var model = new PostIndexModel
        {
            Id = post.Id,
            Title = post.Title,
            AuthorId = post.User.Id,
            AuthorName = post.User.UserName,
            AuthorImageUrl = post.User.ProfileImageUrl,
            AuthorRating = post.User.Rating,
            Created = post.Created,
            PostContent = post.Content,
            Replies = replies,
            ForumId = post.Forum.Id,
            ForumName = post.Forum.Title,
            IsAuthorAdmin = IsAuthorAdmin(post.User)
        };
        return View(model);
    }


    [HttpGet]
    public IActionResult Create(int id)
    {
        var forum = _forumService.GetById(id);
        var model = new NewPostModel
        {
            ForumName = forum.Title,
            ForumId = forum.Id,
            ForumImageUrl = forum.ImageUrl,
            AuthorName = User.Identity?.Name ?? "Unknown",
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddPost(NewPostModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        var post = BuildPost(model, user);
        await _postService.Add(post);
        //TODO: Implement User Rating Management
        return RedirectToAction("Index", "Post", new { post.Id });
    }

    private bool IsAuthorAdmin(ApplicationUser postUser)
    {
        return _userManager.GetRolesAsync(postUser).Result.Contains("Admin");
    }

    private IEnumerable<PostRelyModel> BuildPostReplies(IEnumerable<PostReply> replies)
    {
        return replies.Select(r => new PostRelyModel
        {
            Id = r.Id,
            AuthorName = r.User.UserName,
            AuthorId = r.User.Id,
            AuthorImageUrl = r.User.ProfileImageUrl,
            AuthorRating = r.User.Rating,
            Created = r.Created,
            RelyContent = r.Content,
            IsAuthorAdmin = IsAuthorAdmin(r.User)
        });
    }

    private Post BuildPost(NewPostModel model, ApplicationUser user)
    {
        var forum = _forumService.GetById(model.ForumId);
        return new Post
        {
            Title = model.Title,
            Content = model.Content,
            Created = DateTime.Now,
            User = user,
            Forum = forum
        };
    }
}
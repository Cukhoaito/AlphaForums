using AlphaForums.Data;
using AlphaForums.Data.Models;
using AlphaForums.Models.PostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace AlphaForums.Controllers;

public class PostController : Controller
{
    private readonly IPost _postService;
    private readonly IForum _forumService;
    private readonly IApplicationUser _userService;
    private readonly UserManager<ApplicationUser> _userManager;

    public PostController(IPost postService, IForum forumService, UserManager<ApplicationUser> userManager,
        IApplicationUser userService)
    {
        _postService = postService;
        _forumService = forumService;
        _userManager = userManager;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Index(int id, int? page, int? pageSize)
    {
        page ??= 1;
        pageSize ??= 5;
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
            Replies = replies.ToPagedList((int)page, (int)pageSize),
            ForumId = post.Forum.Id,
            ForumName = post.Forum.Title,
            IsAuthor = IsAuthor(post.User)
        };
        return View(model);
    }


    [HttpGet]
    [Authorize]
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
    [Authorize]
    public async Task<IActionResult> AddPost(NewPostModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return View("Error");
        var post = BuildPost(model, user);
        await _postService.Add(post);
        await _userService.UpdateUserRating(user.Id, typeof(Post));
        return RedirectToAction("Index", "Post", new { post.Id });
    }

    private bool IsAuthor(ApplicationUser postUser)
    {
        return  _userManager.GetUserId(User) == postUser.Id;
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
            ReplyContent = r.Content,
            IsAuthor = IsAuthor(r.User)
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

    public async Task<IActionResult> Delete(int id)
    {
        var post = _postService.GetById(id);
        if (post == null) return View("Error");
        await _postService.Delete(id);
        return RedirectToAction("Topic", "Forum", new { id = post.Forum.Id });
    }

    public IActionResult Edit(int id)
    {
        var post = _postService.GetById(id);
        if (post == null) return View("Error");
        var model = new UpdatePostModel
        {
            PostId = post.Id,
            ForumName = post.Forum.Title,
            ForumId = post.Forum.Id,
            ForumImageUrl = post.Forum.ImageUrl,
            AuthorName = User.Identity?.Name ?? "Unknown",
            Title = post.Title,
            Content = post.Content
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditPost(UpdatePostModel model)
    {
        await _postService.EditPost(model.PostId, model.Title, model.Content);
        return RedirectToAction("Topic", "Forum", new { id = model.ForumId });
    }
}
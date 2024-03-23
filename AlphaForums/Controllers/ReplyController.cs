using AlphaForums.Data;
using AlphaForums.Data.Models;
using AlphaForums.Models.PostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlphaForums.Controllers;

[Authorize]
public class ReplyController : Controller
{
    private readonly IPost _postService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationUser _userService;


    public ReplyController(IPost postService, UserManager<ApplicationUser> userManager, IApplicationUser userService)
    {
        _postService = postService;
        _userManager = userManager;
        _userService = userService;
    }

    public async Task<IActionResult> Create(int id)
    {
        var post = _postService.GetById(id);
        if (User.Identity == null) return View("Error");
        var user = await _userManager.FindByNameAsync(User.Identity.Name??"");
        if(user == null)  return View("Error");
        var model = new PostRelyModel
        {
            PostContent = post.Content,
            PostTitle = post.Title,
            PostId = post.Id,
            AuthorId = user.Id,
            AuthorName = User.Identity.Name,
            AuthorImageUrl = user.ProfileImageUrl,
            AuthorRating = user.Rating,
            IsAuthor = User.IsInRole("Admin"),
            
            ForumName = post.Forum.Title,
            ForumId = post.Forum.Id,
            ForumImageUrl = post.Forum.ImageUrl,
            Created = DateTime.Now
            
        };
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> AddReply(PostRelyModel model)
    {
        var userId = _userManager.GetUserId(User);
        if(userId == null) return View("Error");
        var user = await _userManager.FindByIdAsync(userId);
        var reply = BuildReply(model, user);
        await _postService.AddReply(reply);
        await _userService.UpdateUserRating(userId, typeof(PostReply));
        return RedirectToAction("Index", "Post", new { id = model.PostId });
    }

    private PostReply BuildReply(PostRelyModel model, ApplicationUser user)
    {
        var post = _postService.GetById(model.PostId);
        return new PostReply
        {
            Post = post,
            Content = model.ReplyContent,
            Created = DateTime.Now,
            User = user
        };
    }
}
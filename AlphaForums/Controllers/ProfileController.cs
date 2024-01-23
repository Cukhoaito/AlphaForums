using AlphaForums.Data;
using AlphaForums.Data.Models;
using AlphaForums.Models.ProfileViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AlphaForums.Controllers;

public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationUser _userService;
    private readonly IUpload _uploadService;

    public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser userService,
        IUpload uploadService)
    {
        _userManager = userManager;
        _userService = userService;
        _uploadService = uploadService;
    }

    [HttpGet]
    public async Task<IActionResult> Detail(string id)
    {
        var user = _userService.GetById(id);
        var roles = await _userManager.GetRolesAsync(user);

        var model = new ProfileModel
        {
            UserId = user.Id,
            UserName = user.UserName,
            UserRating = user.Rating.ToString(),
            Email = user.Email,
            ProfileImageUrl = user.ProfileImageUrl,
            MemberSince = user.MemberSince,
            IsAdmin = roles.Contains("Admin")
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadProfileImage(IFormFile file)
    {
        var userId = _userManager.GetUserId(User);
        return View("Error");
    }
}
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
    private readonly IWebHostEnvironment _webHost;

    public ProfileController(UserManager<ApplicationUser> userManager,
        IApplicationUser userService,
        IWebHostEnvironment webHost
    )
    {
        _userManager = userManager;
        _userService = userService;
        _webHost = webHost;
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
        var uploadsFolder = Path.Combine(_webHost.WebRootPath, "images", "users");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }
        
        var fileName = Path.GetFileName("user"+userId + ".png");
        var fileSavePath = Path.Combine(uploadsFolder, fileName);
        await using (var stream = new FileStream(fileSavePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
            await _userService.SetProfileImage(userId, $"/images/users/{fileName}");
        }
        ViewBag.Message = fileName + " upload successfully";
        
        return RedirectToAction("Detail", "Profile", new { id = userId });
    }
}
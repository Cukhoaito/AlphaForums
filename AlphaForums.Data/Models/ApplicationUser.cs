using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace AlphaForums.Data.Models;

public class ApplicationUser : IdentityUser
{
    public int Rating { get; set; }
    [DefaultValue("/images/users/defaultAvatar.png")]
    public string ProfileImageUrl { get; set; } = string.Empty;
    public DateTime MemberSince { get; set; }
    public bool IsActive { get; set; }
}
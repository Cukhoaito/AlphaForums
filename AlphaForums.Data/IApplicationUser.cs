using AlphaForums.Data.Models;

namespace AlphaForums.Data;

public interface IApplicationUser
{
    ApplicationUser GetById(string id);
    IEnumerable<ApplicationUser> GetAll();
    Task SetProfileImage(string id, string url);
    Task UpdateUserRating(string id, Type type);
}
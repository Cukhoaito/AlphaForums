using AlphaForums.Data;
using AlphaForums.Data.Models;

namespace AlphaForums.Service;

public class ApplicationUserService : IApplicationUser
{
    private readonly ApplicationDbContext _context;

    public ApplicationUserService(ApplicationDbContext context)
    {
        _context = context;
    }
    public ApplicationUser GetById(string id)
    {
        return GetAll().FirstOrDefault(user => user.Id.Equals(id));
    }

    public IEnumerable<ApplicationUser> GetAll()
    {
        return _context.ApplicationUsers;
    }

    public async Task SetProfileImage(string id, string url)
    {
        var user = GetById(id);
        user.ProfileImageUrl = url;
        _context.Update(user);
        await _context.SaveChangesAsync();
    }

    public  async Task UpdateUserRating(string userId, Type type)
    {
        var user = GetById(userId);
        if (user == null) return;
        user.Rating = CalculateUserRating(type, user.Rating);
        await _context.SaveChangesAsync();
    }

    private int CalculateUserRating(Type type, int userRating)
    {
        var inc = 0;
        if (type == typeof(Post))
            inc = 1;
        if (type == typeof(PostReply))
            inc = 3;
        return userRating + inc;
    }
}
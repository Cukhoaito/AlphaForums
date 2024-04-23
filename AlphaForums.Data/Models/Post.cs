using System.ComponentModel.DataAnnotations;

namespace AlphaForums.Data.Models;

public class Post
{
    public int Id { get; set; }
    [MaxLength(255)]
    public string Title { get; set; } = String.Empty;
    public string Content { get; set; }
    
    public bool Enable { get; set; }
    public DateTime Created { get; set; }

    public Forum Forum { get; set; }
    public ApplicationUser User { get; set; }  
    public IEnumerable<PostReply> Relies { get; set; }
}
namespace BoiteAIdees.Models;

public partial class Users
{
    public int UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public bool IsAdmin { get; set; }

    public virtual ICollection<Ideas> Ideas { get; set; } = new List<Ideas>();

    public virtual ICollection<UserLikedIdeas> UserLikedIdeas { get; set; } = new List<UserLikedIdeas>();
}
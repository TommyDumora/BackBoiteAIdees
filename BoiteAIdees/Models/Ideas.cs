namespace BoiteAIdees.Models;

public partial class Ideas
{
    public int IdeaId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int CategoryId { get; set; }

    public int UserId { get; set; }

    public virtual Categories Category { get; set; }

    public virtual Users User { get; set; }

    public virtual ICollection<UserLikedIdeas> UserLikedIdeas { get; set; } = new List<UserLikedIdeas>();
}
namespace BoiteAIdees.Models;

public partial class UserLikedIdeas
{
    public int UserId { get; set; }

    public int IdeaId { get; set; }

    public virtual Ideas Idea { get; set; }

    public virtual Users User { get; set; }
}
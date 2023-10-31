namespace BoiteAIdees.Models;

public partial class Categories
{
    public int CategoryId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Ideas> Ideas { get; set; } = new List<Ideas>();
}
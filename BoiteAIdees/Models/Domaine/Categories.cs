namespace BoiteAIdees.Models.Domaine;

/// <summary>
/// Représente une catégorie.
/// </summary>
public partial class Categories
{
    /// <summary>
    /// Obtient ou définit l'identifiant de la catégorie.
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Obtient ou définit le nom de la catégorie.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des idées associées à cette catégorie.
    /// </summary>
    public virtual ICollection<Ideas> Ideas { get; set; } = new List<Ideas>();
}
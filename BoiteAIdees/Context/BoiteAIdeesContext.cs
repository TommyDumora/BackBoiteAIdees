using BoiteAIdees.Models.Domaine;
using Microsoft.EntityFrameworkCore;

namespace BoiteAIdees.Context
{
    /// <summary>
    /// Représente le contexte de base de données pour la Boite à Idées.
    /// </summary>
    public partial class BoiteAIdeesContext : DbContext
    {
        /// <summary>
        /// Constructeur pour le contexte de base de données Boite à Idées.
        /// </summary>
        /// <param name="options">Options du contexte de base de données.</param>
        public BoiteAIdeesContext(DbContextOptions<BoiteAIdeesContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Représente l'ensemble des données pour les catégories.
        /// </summary>
        public DbSet<Categories> Categories { get; set; }

        /// <summary>
        /// Représente l'ensemble des données pour les idées.
        /// </summary>
        public DbSet<Ideas> Ideas { get; set; }

        /// <summary>
        /// Représente l'ensemble des données pour les relations "utilisateur a aimé une idée".
        /// </summary>
        public DbSet<UserLikedIdeas> UserLikedIdeas { get; set; }

        /// <summary>
        /// Représente l'ensemble des données pour les utilisateurs.
        /// </summary>
        public DbSet<Users> Users { get; set; }

        /// <summary>
        /// Méthode pour configurer le modèle de données et les entités.
        /// </summary>
        /// <param name="modelBuilder">Constructeur de modèles pour configurer les entités.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B7119A3BA");

                entity.HasIndex(e => e.Name, "UQ__Categori__737584F61FCEB668").IsUnique();

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Ideas>(entity =>
            {
                entity.HasKey(e => e.IdeaId).HasName("PK__Ideas__FE218223F901EC0B");

                entity.Property(e => e.IdeaId).HasColumnName("IdeaID");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Description).IsRequired();
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Category).WithMany(p => p.Ideas)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ideas__CategoryI__3E52440B");

                entity.HasOne(d => d.User).WithMany(p => p.Ideas)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Ideas__UserID__3F466844");
            });

            modelBuilder.Entity<UserLikedIdeas>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.IdeaId }).HasName("PK__UserLike__386AD48E544A4C55");

                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.IdeaId).HasColumnName("IdeaID");

                entity.HasOne(d => d.Idea).WithMany(p => p.UserLikedIdeas)
                    .HasForeignKey(d => d.IdeaId)
                    .HasConstraintName("FK__UserLiked__IdeaI__4316F928");

                entity.HasOne(d => d.User).WithMany(p => p.UserLikedIdeas)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__UserLiked__UserI__4222D4EF");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACD8ADA1C6");

                entity.Property(e => e.UserId).HasColumnName("UserID");
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(64);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
using BoiteAIdees.Models;
using Microsoft.EntityFrameworkCore;

namespace BoiteAIdees.Context;

public partial class BoiteAIdeesContext : DbContext
{
    public BoiteAIdeesContext(DbContextOptions<BoiteAIdeesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categories> Categories { get; set; }

    public virtual DbSet<Ideas> Ideas { get; set; }

    public virtual DbSet<UserLikedIdeas> UserLikedIdeas { get; set; }

    public virtual DbSet<Users> Users { get; set; }

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
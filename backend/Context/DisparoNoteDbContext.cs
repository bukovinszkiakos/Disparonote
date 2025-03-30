using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class DisparoNoteDbContext : IdentityDbContext<IdentityUser>
{
    public DisparoNoteDbContext(DbContextOptions<DisparoNoteDbContext> options)
        : base(options)
    {
    }
    public DbSet<Note> Notes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Note>()
            .HasIndex(n => n.AccessKey)
            .IsUnique();
    }
}

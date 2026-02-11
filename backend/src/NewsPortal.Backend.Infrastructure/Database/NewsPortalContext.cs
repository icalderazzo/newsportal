using Microsoft.EntityFrameworkCore;
using NewsPortal.Backend.Domain.Models.Items;

namespace NewsPortal.Backend.Infrastructure.Database;

public class NewsPortalContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<UserItem> UserItems { get; set; }

    public NewsPortalContext(DbContextOptions options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Id).ValueGeneratedOnAdd();
            entity.Property(u => u.FirstName).HasMaxLength(20);
            entity.Property(u => u.LastName).HasMaxLength(30);
            entity.Property(u => u.Email).HasMaxLength(50);
        });
        
        modelBuilder.Entity<Item>(entity =>
        {
            entity.Property(i => i.Id).ValueGeneratedNever();
            
            entity
                .HasDiscriminator<int>("ItemType")
                .HasValue<Item>((int)ItemTypes.Unknown)
                .HasValue<Story>((int)ItemTypes.Story);
        });
        
        modelBuilder.Entity<UserItem>(entity =>
        {
            entity.HasKey(ui => new { ui.UserId, ui.ItemId });
            entity.Property(ui => ui.SavedAt).HasDefaultValueSql("GETDATE()");

            entity
                .HasOne(ui => ui.User)
                .WithMany(ui => ui.UserItems)
                .HasForeignKey(ui => ui.UserId);
            
            entity
                .HasOne(ui => ui.Item)
                .WithMany(i => i.UserItems)
                .HasForeignKey(ui => ui.ItemId);
        });
    }
}
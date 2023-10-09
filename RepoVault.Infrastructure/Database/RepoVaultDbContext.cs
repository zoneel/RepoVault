using Microsoft.EntityFrameworkCore;

namespace RepoVault.Infrastructure.Database;

public class RepoVaultDbContext : DbContext
{
    public DbSet<BackupLog> BackupLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BackupLog>()
            .ToTable("BackupLogs")
            .HasKey(b => b.Id);

        modelBuilder.Entity<BackupLog>()
            .Property(b => b.RepositoryName)
            .HasColumnName("REPOSITORY_NAME");

        modelBuilder.Entity<BackupLog>()
            .Property(b => b.BackupDate)
            .HasColumnName("DATE");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=RepoVault.db");
    }
}
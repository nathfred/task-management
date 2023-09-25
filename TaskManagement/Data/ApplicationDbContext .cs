namespace TaskManagement.Data;

using Microsoft.EntityFrameworkCore;
using TaskManagement.Models;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

public class ApplicationDbContext : DbContext
{
    public DbSet<Task> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityUserRole<string>> UserRoles { get; set; }
    public DbSet<IdentityUserClaim<string>> UserClaims { get; set; }
    public DbSet<IdentityUserLogin<string>> UserLogins { get; set; }
    public DbSet<IdentityUserToken<string>> UserTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships, keys, constraints, etc.
        modelBuilder.Entity<Task>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId);

        // Configure ASP.NET Core Identity entities
        modelBuilder.Entity<IdentityRole>().ToTable("Roles");
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles").HasKey(p => new { p.UserId, p.RoleId });
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins").HasKey(p => new { p.LoginProvider, p.ProviderKey });
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens").HasKey(p => new { p.UserId, p.LoginProvider, p.Name });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Replace "your_connection_string" with your actual connection string
        string connectionString = "Server=localhost\\SQLEXPRESS03;Database=TaskManagement;User=sa;Password=Q1w2e3r4t;";

        // Disable SSL validation (not recommended for production)
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
        builder.TrustServerCertificate = true; // This line disables SSL certificate validation

        optionsBuilder.UseSqlServer(builder.ConnectionString);
    }

    private static bool ValidateCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        // Allow all certificates without validation
        return true;
    }
}

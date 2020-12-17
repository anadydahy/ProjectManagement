using Microsoft.EntityFrameworkCore;
namespace ProjectManagement.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<UserProject> UserProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProject>()
                .HasKey(userProject => new { userProject.UserId, userProject.ProjectId });
            modelBuilder.Entity<UserProject>()
                .HasOne(userProject => userProject.User)
                .WithMany(user => user.UserProjects)
                .HasForeignKey(userProject => userProject.UserId);
            modelBuilder.Entity<UserProject>()
                .HasOne(userProject => userProject.Project)
                .WithMany(project => project.UserProjects)
                .HasForeignKey(userProject => userProject.ProjectId);

            modelBuilder.Entity<User>()
            .HasAlternateKey(u => u.Email)
            .HasName("AlternateKey_Email");

            base.OnModelCreating(modelBuilder);
        }
    }
}


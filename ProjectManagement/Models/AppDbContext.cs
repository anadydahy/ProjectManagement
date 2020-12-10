using Microsoft.EntityFrameworkCore;
namespace ProjectManagement.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        public DbSet<Project> Project { get; set; }

        public DbSet<Ticket> Ticket { get; set; }

        public DbSet<UserProject> UserProject { get; set; }

        public DbSet<TicketProject> TicketProject { get; set; }

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

            modelBuilder.Entity<TicketProject>()
                .HasKey(ticketProject => new { ticketProject.TicketId, ticketProject.ProjectId });
            modelBuilder.Entity<TicketProject>()
                .HasOne(ticketProject => ticketProject.Ticket)
                .WithMany(ticket => ticket.TicketProjects)
                .HasForeignKey(ticketProject => ticketProject.TicketId);
            modelBuilder.Entity<TicketProject>()
                .HasOne(ticketProject => ticketProject.Project)
                .WithMany(project => project.TicketProjects)
                .HasForeignKey(ticketProject => ticketProject.ProjectId);

            //make email Unique as there no data annotations in .netcore for it
            modelBuilder.Entity<User>()
            .HasAlternateKey(u => u.Email)
            .HasName("AlternateKey_Email");

            base.OnModelCreating(modelBuilder);
        }
    }
}


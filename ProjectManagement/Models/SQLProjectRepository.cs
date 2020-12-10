namespace ProjectManagement.Models
{
    public class SQLProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public SQLProjectRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}

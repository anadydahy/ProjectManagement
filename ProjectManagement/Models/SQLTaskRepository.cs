namespace ProjectManagement.Models
{
    public class SQLTaskRepository
    {
        private readonly AppDbContext _context;

        public SQLTaskRepository(AppDbContext context)
        {
            _context = context;
        }
    }
}

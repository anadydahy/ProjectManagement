namespace ProjectManagement.Models
{
    public class TicketProject
    {
        public int TicketId { get; set; }

        public int ProjectId { get; set; }

        public Project Project { get; set; }

        public Ticket Ticket { get; set; }
    }
}

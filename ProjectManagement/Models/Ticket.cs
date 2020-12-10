using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name can't exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Status { get; set; }

        public User user { get; set; }

        public ICollection<TicketProject> TicketProjects { get; set; }

    }
}

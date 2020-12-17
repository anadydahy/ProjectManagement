using ProjectManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.ViewModels
{
    public class AddTicket
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name can't exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool Status { get; set; }

        public int UserId { get; set; }

        public int ProjectId { get; set; }

    }
}

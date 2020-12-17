using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.ViewModels
{
    public class AddProject
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name can't exceed 50 characters")]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Status { get; set; } = false;
    }
}

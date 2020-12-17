using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.ViewModels
{
    public class ProjectDetails
    {
        public Project Project { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}

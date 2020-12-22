using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.ViewModels
{
    public class IndexView
    {
        public List<Project> Projects { get; set; }

        public List<UserProject> UserProjects { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Territory.Models.ViewModel
{
    public class UserVM
    {
        public IEnumerable<ApplicationUser> ApplicationUser { get; set; }
        public string SearchTerm { get; set; }
    }
}

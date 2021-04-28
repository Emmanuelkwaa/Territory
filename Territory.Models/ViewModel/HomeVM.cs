using System;
using System.Collections.Generic;
using System.Text;

namespace Territory.Models.ViewModel
{
    public class HomeVM
    {
        public IEnumerable<Territory> Territory { get; set; }
        public ApplicationUser ApplicationUsers { get; set; }
        public string SearchTerm { get; set; }
        public int Filter { get; set; }
    }
}

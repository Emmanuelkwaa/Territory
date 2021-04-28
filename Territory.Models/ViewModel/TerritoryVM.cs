using System;
using System.Collections.Generic;
using System.Text;

namespace Territory.Models.ViewModel
{
    public class TerritoryVM
    {
        public IEnumerable<Territory> Territory { get; set; }
        public string SearchTerm { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Territory.Models.ViewModel
{
    public class UserTerritoryListVM
    {
        public IEnumerable<PublisherTerritory> PublisherTerritories { get; set; }
        public ApplicationUser ApplicationUsers { get; set; }
    }
}

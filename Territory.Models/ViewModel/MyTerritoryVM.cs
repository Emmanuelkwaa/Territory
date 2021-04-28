using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Territory.Models.ViewModel
{
    public class MyTerritoryVM
    {
        public IEnumerable<PublisherTerritory> PublisherTerritories { get; set; }
        public IEnumerable<Publication> Publication { get; set; }
        public IEnumerable<Note> Note { get; set; }
    }
}

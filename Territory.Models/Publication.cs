using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Territory.Models
{
    public class Publication
    {
        public int Id { get; set; }
        public string NameOfPublication { get; set; }
        public DateTime DatePlaced { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public int PublisherTerritoryId { get; set; }
        [ForeignKey("PublisherTerritoryId")]
        public PublisherTerritory PublisherTerritory { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Territory.Models
{
    public class PublisherTerritory
    {
        [Key]
        public int Id { get; set; }
        public DateTime LastVisited { get; set; }
        public bool IsNotGhanaian { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int TerritoryId { get; set; }
        [ForeignKey("TerritoryId")]
        public Territory Territory { get; set; }
    }
}

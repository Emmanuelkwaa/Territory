using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Territory.Models
{
    public class Territory
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Apartment { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsTaken { get; set; }
        public string Comment { get; set; }
        public string CommentBy { get; set; }
        public string CurrentPub { get; set; }
        public DateTime DateCurrentPub { get; set; }
        public string LastPub { get; set; }
        public DateTime DateLastPub { get; set; }
    }
}

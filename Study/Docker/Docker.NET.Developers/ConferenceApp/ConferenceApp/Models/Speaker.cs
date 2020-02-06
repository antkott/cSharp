using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceApp.Models
{
    public class Speaker
    {
        [Key]
        public int Id { get; set; }
        public string First { get; set; }
        public string Last { get; set; }
        public string EmailAddress { get; set; }
        public string FullName
        {
            get
            {
                return $"{this.First} {this.Last}";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceApp.Models
{
    public class Sponsor
    {
        [Key]
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Level { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}

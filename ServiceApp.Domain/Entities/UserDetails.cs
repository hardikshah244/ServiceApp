using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class UserDetails
    {        
        [Key]
        public string UserID { get; set; }     
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]        
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }
    }
}

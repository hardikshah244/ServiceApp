using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.DataModel
{
    internal sealed class EngineerInfoMetadata
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Please enter valid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Required]
        [MaxLength(100)]
        public string State { get; set; }

        [Required]
        [MaxLength(15)]
        public string Pincode { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        [Required]
        [MaxLength(50)]
        public string DeviceID { get; set; }

        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(EngineerInfoMetadata))]
    public partial class EngineerInfo
    {

    }
}

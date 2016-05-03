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
        public string MembershipType { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",
               ApplyFormatInEditMode = true)]
        public System.DateTime CreationDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",
             ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",
             ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EndDate { get; set; }

        public Nullable<decimal> Amount { get; set; }

        public string Id { get; set; }
    }

    [MetadataType(typeof(EngineerInfoMetadata))]
    public partial class EngineerInfo
    {

    }
}

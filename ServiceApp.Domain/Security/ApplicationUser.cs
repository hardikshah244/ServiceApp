using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace ServiceApp.Domain.Security
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Area { get; set; }

        public string SubArea { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Pincode { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public string DeviceID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class CustomerProfileInfo
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Area { get; set; }
        public string SubArea { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
    }
}

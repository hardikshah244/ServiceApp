using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class DeviceTokenRequest
    {
        public string Email { get; set; }
        public string DeviceID { get; set; }
    }
}

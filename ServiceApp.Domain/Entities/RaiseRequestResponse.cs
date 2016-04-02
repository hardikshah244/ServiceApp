using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class RaiseRequestResponse
    {
        public int ServiceRequestID { get; set; }
        public string Message { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
    }
}

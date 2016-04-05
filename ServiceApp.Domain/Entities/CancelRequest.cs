using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class CancelRequest
    {
        public int ServiceRequestID { get; set; }
        public int StatusTypeID { get; set; }
        public string ServiceRequestRemark { get; set; }
    }
}

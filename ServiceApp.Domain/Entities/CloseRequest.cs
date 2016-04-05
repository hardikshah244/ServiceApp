using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class CloseRequest
    {
        public int ServiceRequestID { get; set; }
        public int StatusTypeID { get; set; }
    }
}

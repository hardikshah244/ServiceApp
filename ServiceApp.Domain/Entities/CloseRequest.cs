using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class CloseRequest
    {
        public string ServiceRequestNO { get; set; }
        public int StatusTypeID { get; set; }
    }
}

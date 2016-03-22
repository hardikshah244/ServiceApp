using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class RaiseRequest
    {
        public int ServiceTypeID { get; set; }
        public int StatusTypeID { get; set; }
        public string Landmark { get; set; }
        public string Remark { get; set; }
        public string CreatedUserID { get; set; }
    }
}

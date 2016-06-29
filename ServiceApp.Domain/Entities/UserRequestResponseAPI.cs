using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class UserRequestResponseAPI
    {
        public string ServiceRequestNO { get; set; }
        public string ServiceTypeName { get; set; }
        public string ServiceCategoryName { get; set; }
        public string StatusTypeName { get; set; }
        public string Landmark { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Name { get; set; }
        public int StatusTypeID { get; set; }
    }
}

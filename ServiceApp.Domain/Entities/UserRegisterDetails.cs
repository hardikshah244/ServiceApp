using ServiceApp.Domain.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class UserRegisterDetails
    {
        public RegisterUser User { get; set; }
        public UserDetail UserDetails { get; set; }
    }
}

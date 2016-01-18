using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Entities
{
    public class UserRegisterDetails
    {
        public User User { get; set; }
        public UserDetails UserDetails { get; set; }
    }
}

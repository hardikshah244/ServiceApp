using ServiceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Abstract
{
    public interface IUserDetailRepository : IDisposable
    {
        void AddUserDetail(UserDetails userDetails);
    }
}

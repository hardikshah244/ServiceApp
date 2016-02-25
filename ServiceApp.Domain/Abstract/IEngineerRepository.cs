using ServiceApp.Domain.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Abstract
{
    public interface IEngineerRepository : IDisposable
    {
        IEnumerable<Engineer> GetEngineerDetailsByLocality(string Locality, string SubLocality, string City, string State, string Pincode,
            decimal Latitude, decimal Longitude);
    }
}


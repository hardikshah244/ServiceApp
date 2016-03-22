using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Security
{
    public class OwinAuthDbContext : IdentityDbContext<ApplicationUser>
    {
        public OwinAuthDbContext()
            : base("name=OwinAuthDbContext")
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<OwinAuthDbContext>());
            this.Configuration.ProxyCreationEnabled = false;
        }

        public static OwinAuthDbContext Create()
        {
            return new OwinAuthDbContext();
        }
    }
}

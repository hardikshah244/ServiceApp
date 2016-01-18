using ServiceApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceApp.Domain.Concrete
{
    public class OwinAuthDbContext : DbContext
    {
        public OwinAuthDbContext()
            : base("OwinAuthDbContext")
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<OwinAuthDbContext>());            
        }

        public DbSet<UserDetails> UserDetail { get; set; }
    }
}

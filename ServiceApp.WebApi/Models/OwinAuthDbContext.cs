﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ServiceApp.WebApi.Models
{
    public class OwinAuthDbContext : IdentityDbContext
    {
        public OwinAuthDbContext()
            : base("OwinAuthDbContext")
        {
            //Database.SetInitializer(new CreateDatabaseIfNotExists<OwinAuthDbContext>());
        }
    }
}
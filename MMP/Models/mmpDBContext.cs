using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MMP.Models
{
    public class mmpDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyCategory> PropertyCategories { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Advice> Advices { get; set; }

        public DbSet<Package> Packages { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace DonorApp.Models
{
    public partial class SangamDBContext: DbContext
    {
        public SangamDBContext() : base("SangamDBContext")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        public virtual DbSet<ThidiMaster> ThidiMasters { get; set; }
        public virtual DbSet<MonthMaster> MonthMasters { get; set; }
        public virtual DbSet<DonarDetail> DonarDetails { get; set; }
    }
}
using CasperJsSpider.SqlLiteDomain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain
{
    public class SpiderContext: DbContext
    {
        public SpiderContext(string databaseName = "Spider")
           : base(databaseName)
        {
        }

        public DbSet<Catalog> Catalogs { set; get; }

        public DbSet<Product> Products { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { }
    }
}

using CasperJsSpider.SqlLiteDomain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpiderApp
{
    public class SpiderContext: DbContext
    {
        public SpiderContext(string databaseName = "SpiderEF6")
           : base(databaseName)
        {
        }

        //public DbSet<Catalog> Catalogs { set; get; }

        //public DbSet<Product> Products { set; get; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Type[] thisAllTypes = Assembly.GetExecutingAssembly().GetTypes().Where(o => o.BaseType != null
           && o.BaseType.IsGenericType
           && o.BaseType.GetGenericTypeDefinition() == typeof(System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<>)).ToArray();
            foreach (var type in thisAllTypes)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            base.OnModelCreating(modelBuilder);
        }
    }
}

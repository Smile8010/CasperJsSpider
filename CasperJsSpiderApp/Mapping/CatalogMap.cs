using CasperJsSpider.SqlLiteDomain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpiderApp.Mapping
{
    public class CatalogMap : EntityTypeConfiguration<Catalog>
    {
        public CatalogMap()
        {
            ToTable("Catalog");

            HasKey(t => t.ID);

            Property(t => t.CreateTime).IsRequired();

            Property(t => t.RankTime).IsRequired();
        }
    }
}

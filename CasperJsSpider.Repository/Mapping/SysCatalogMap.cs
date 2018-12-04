using CasperJsSpider.SqlLiteDomain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.Repository.Mapping
{
    public class SysCatalogMap : EntityTypeConfiguration<SysCatalog>
    {
        public SysCatalogMap()
        {
            ToTable("tb_Sys_Catalog");

            HasKey(o => o.ID);
        }
    }
}

using CasperJsSpider.SqlLiteDomain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.Repository.Mapping
{
    public class SysProductMap : EntityTypeConfiguration<SysProduct>
    {
        public SysProductMap()
        {
            ToTable("tb_Sys_Product");

            HasKey(o => o.ID);
        }
    }
}

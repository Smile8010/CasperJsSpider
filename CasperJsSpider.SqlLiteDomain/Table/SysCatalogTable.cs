using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.Table
{
    public class SysCatalogTable
    {
        public string ID { get; set; }

        public string Url { get; set; }

        public string ParentID { get; set; }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public string UpdateString { get; set; }

        public string CatalogProductTableName { get; set; }

        public bool IsDel { get; set; }
    }
}

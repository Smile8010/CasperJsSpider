using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.Table
{
    public class CatalogTable
    {
        public Guid ID { get; set; }

        public string CatalogName { get; set; }

        public string CatalogLink { get; set; }

        public DateTime CreateTime { get; set; }

        public string RankTime { get; set; }
    }
}

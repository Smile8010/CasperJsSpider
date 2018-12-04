using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.ViewModel
{
    public class CatalogProductViewModel
    {
        public string ID { get; set; }

        public string CatalogID { get; set; }

        public string ProductID { get; set; }

        public int RankLevel { get; set; }

        public string RankTime { get; set; }

        public DateTime CreateTime { get; set; }
    }
}

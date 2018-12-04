using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.Table
{
    public class ProductTable
    {
        public Guid ID { get; set; }

        public string ImgPath { get; set; }

        public int RankNumber { get; set; }

        public string Name { get; set; }

        public string Link { get; set; }

        public string Price { get; set; }

        public string Ref { get; set; }

        public string Asin { get; set; }

        public string RefCatalogLink { get; set; }

        public DateTime CreateTime { get; set; }

        public string RankTime { get; set; }
    }
}

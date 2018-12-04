using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.ViewModel
{
    public class ProductListItemQueryViewModel
    {
        public string CatalogID { get; set; }

        public string Name { get; set; }

        public int RankNumber { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

    }
}

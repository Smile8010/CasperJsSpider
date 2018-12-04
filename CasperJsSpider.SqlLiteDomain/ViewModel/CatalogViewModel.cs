using CasperJsSpider.SqlLiteDomain.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.ViewModel
{
    public class CatalogViewModel : CatalogTable
    {
        public CatalogViewModel()
        {
            this.Products = new List<ProductTable>();
        }
        public List<ProductTable> Products { get; set; }
    }
}

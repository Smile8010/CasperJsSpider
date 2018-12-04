using CasperJsSpider.SqlLiteDomain.Model;
using CasperJsSpider.SqlLiteDomain.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.ViewModel
{
    public class SysCatalogViewModel : SysCatalogTable
    {
        public SysCatalogViewModel()
        {
            ChildCatalogs = new List<SysCatalog>();
        }

        public List<SysCatalog> ChildCatalogs { get; set; }
    }
}

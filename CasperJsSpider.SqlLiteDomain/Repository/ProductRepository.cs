using CasperJsSpider.SqlLiteDomain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.Repository
{
    public class ProductRepository : BaseRepository<Product>
    {
        public void AddRange(List<Product> list)
        {
            using (DbContext Context = new SpiderContext())
            {
                list.ForEach(l =>
                {
                    l.ID = Guid.NewGuid().ToString().ToUpper();
                });
                Context.Set<Product>().AddRange(list);
                Save(Context);
            }
        }
    }
}

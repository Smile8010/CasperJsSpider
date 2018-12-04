using CasperJsSpider.SqlLiteDomain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain.Repository
{
    public class CatalogRepository : BaseRepository<Catalog>
    {
        public Catalog AddIfNoExist(Catalog entity)
        {
            using (DbContext db = new SpiderContext())
            {
                var findEntity = db.Set<Catalog>().Where(o => o.RID == entity.RID).FirstOrDefault();
                if (findEntity != null)
                {
                    return findEntity;
                }
                entity.ID = Guid.NewGuid().ToString().ToUpper();
                db.Set<Catalog>().Add(entity);
                Save(db);
                return entity;
            }
        }
    }
}

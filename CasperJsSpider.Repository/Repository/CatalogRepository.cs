using CasperJsSpider.SqlLiteDomain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.Repository.Repository
{
    public class CatalogRepository : BaseRepository<Catalog>
    {
        public Catalog AddIfNoExist(Catalog entity)
        {
            using (DbContext db = DbContextFactory.Context)
            {
              
                var findEntity = db.Set<Catalog>().Where(o => o.CatalogLink == entity.CatalogLink).FirstOrDefault();
                if (findEntity != null)
                {
                    return findEntity;
                }
                entity.ID = Guid.NewGuid();
                entity.CreateTime = DateTime.Now;
                db.Set<Catalog>().Add(entity);
                Save(db);
                return entity;
            }
        }
    }
}

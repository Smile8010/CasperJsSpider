using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpiderApp.Repository
{
    public class BaseRepository<T>  where T:class
    {
        public void Add(T entity)
        {
            using (DbContext Context = new SpiderContext())
            {
                Context.Set<T>().Add(entity);
                Save(Context);
            } 
        }

        public void Save(DbContext Context)
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw new Exception(e.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.Repository.Repository
{
    public class BaseRepository<T> where T : class
    {

        public T FindSingle(Expression<Func<T, bool>> exp)
        {
            using (DbContext Context = DbContextFactory.Context)
            {
                return Context.Set<T>().AsNoTracking().FirstOrDefault(exp);
            }
        }

        /// <summary>
        /// 根据过滤条件，获取记录
        /// </summary>
        /// <param name="exp">The exp.</param>
        public List<T> Find(Expression<Func<T, bool>> exp = null)
        {
            using (DbContext Context = DbContextFactory.Context)
            {
                return Filter(Context, exp).ToList();
            }
        }

        public void Add(T entity)
        {
            using (DbContext Context = DbContextFactory.Context)
            {
                Context.Set<T>().Add(entity);
                Save(Context);
            }
        }

        public void AddRange(List<T> list)
        {
            using (DbContext Context = DbContextFactory.Context)
            {
                Context.Set<T>().AddRange(list);
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

        public IQueryable<T> Filter(DbContext Context, Expression<Func<T, bool>> exp)
        {
            var dbSet = Context.Set<T>().AsQueryable();
            if (exp != null)
                dbSet = dbSet.Where(exp);
            return dbSet;
        }
    }
}

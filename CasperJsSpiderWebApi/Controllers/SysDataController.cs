using CasperJsSpider.Repository;
using CasperJsSpider.Repository.Repository;
using CasperJsSpider.SqlLiteDomain;
using CasperJsSpider.SqlLiteDomain.Model;
using CasperJsSpider.SqlLiteDomain.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Web.Http;

namespace CasperJsSpiderWebApi.Controllers
{
    /// <summary>
    /// 业务数据
    /// </summary>
    public class SysDataController : ApiController
    {
        /// <summary>
        /// 获取分类菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/menu")]
        public Result<object> GetSysCatalogList()
        {
            SysCatalogRepository dal = new SysCatalogRepository();
            List<SysCatalog> list = dal.Find(o => !o.IsDel);
            List<SysCatalog> parentList = list.Where(o => o.ParentID == null).ToList();
            var response = parentList.ConvertAll(o => new
            {
                Name = o.Name,
                ID = o.ID,
                SubMenuList = list.Where(l => l.ParentID == o.ID).Select(l => new
                {
                    l.Name,
                    l.ID
                }).ToList()
            });

            return new Result<object>(response);
        }

        /// <summary>
        /// 获取某一分类下一周的产品
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="name"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        [HttpGet, Route("api/searchproductname")]
        public Result<object> GetSysCatalogProductList(string catalogId, string name, DateTime? startDate, DateTime? endDate, int start = 0, int limit = 6)
        {
            Result<object> result = new Result<object>(true);

            if (string.IsNullOrEmpty(name))
            {
                result.Success = false;
                result.Msg = "请输入要查询的产品名称!";
                return result;
            }

            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.AddDays(-7);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now;
            }

            using (DbContext db = DbContextFactory.Context)
            {
                //找出mapName
                var dbSet = db.Set<SysCatalog>();
                SysCatalog entity = dbSet.First(o => o.ID == catalogId);

                //string querySql = $@"SELECT DISTINCT p.ID
                //                     FROM {entity.CatalogProductTableName} map 
                //                     JOIN tb_Sys_Product p ON map.ProductID = p.ID
                //                     WHERE p.CatalogID = @CatalogID AND p.Name LIKE @Name AND map.CreateTime BETWEEN '{startDate.Value.ToString("yyyy-MM-dd")} 00:00:00' AND '{endDate.Value.ToString("yyyy-MM-dd")} 23:59:59'  
                //                     ORDER BY map.CreateTime DESC
                //                     LIMIT 6 ";
                //List<string> queryIDs = db.Database.SqlQuery<string>(querySql, new SQLiteParameter[] {
                //      new SQLiteParameter("@CatalogID",catalogId),
                //      new SQLiteParameter("@Name",$"%{name}%")
                //}).ToList();

                string fromSql, whereSql;

                fromSql = "FROM tb_Sys_Product p ";

                List<SQLiteParameter> SQLiteParamList = new List<SQLiteParameter>() {
                     new SQLiteParameter("@CatalogID",catalogId)
                };
                whereSql = "WHERE p.CatalogID = @CatalogID ";
                if (!string.IsNullOrEmpty(name))
                {
                    SQLiteParamList.Add(new SQLiteParameter("@Name", $"%{name}%"));
                    whereSql += "AND p.Name LIKE @Name ";
                }

                
                whereSql += $"AND EXISTS ( SELECT 1 FROM  {entity.CatalogProductTableName} map WHERE map.ProductID = p.ID AND map.CreateTime BETWEEN '{startDate.Value.ToString("yyyy-MM-dd")} 00:00:00' AND '{endDate.Value.ToString("yyyy-MM-dd")} 23:59:59' ) ";


                string querySql = $@"SELECT p.ID {fromSql} {whereSql}
                                     ORDER BY p.CreateTime
                                     LIMIT {limit}
                                     OFFSET {start};";

                SQLiteParameter[] SQLiteParameters = SQLiteParamList.ToArray();

                List<string> queryIDs = db.Database.SqlQuery<string>(querySql, SQLiteParameters).ToList();
                result.Total = 0;
                if (queryIDs.Count > 0)
                {
                    querySql = $@"SELECT p.ID,p.Name,p.ImgPath
                              FROM tb_Sys_Product p
                              WHERE p.ID IN ( '{string.Join("','", queryIDs.ToArray())}' );";
                    result.Data = db.Database.SqlQuery<SysCatalogProductNameWeekRanking>(querySql).ToList();
                    querySql = $"SELECT COUNT(1) {fromSql} {whereSql};";
                    result.Total = db.Database.SqlQuery<int>(querySql, SQLiteParameters).FirstOrDefault();
                }
                else
                {
                    result.Data = new List<string>();

                }
                //result.Data = db.Database.SqlQuery<SysCatalogProductNameWeekRanking>(querySql, new SQLiteParameter[] {
                //      new SQLiteParameter("@CatalogID",catalogId),
                //      new SQLiteParameter("@Name",$"%{name}%")
                //}).ToList();
                return result;
            }

        }

        /// <summary>
        /// 获取某个分类下产品一周的排名情况
        /// lihd 2018-12-17
        /// </summary>
        /// <param name="catalogId">分类id</param>
        /// <param name="productId">产品Id</param>
        /// <param name="startDate">开始时间</param>
        /// <returns></returns>
        [HttpGet, Route("api/searchproductweekchart")]
        public Result<object> GetSysCatalogProductWeekRanking(string catalogId, string productId, DateTime? startDate, DateTime? endDate)
        {
            Result<object> result = new Result<object>(true);

            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.AddDays(-7);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now;
            }


            //找出所有符合的产品
            using (DbContext db = DbContextFactory.Context)
            {
                //找出mapName
                var dbSet = db.Set<SysCatalog>();
                SysCatalog entity = dbSet.First(o => o.ID == catalogId);

                string querySql = $@"SELECT map.RankLevel,map.CreateTime 
                                     FROM {entity.CatalogProductTableName} map 
                                     WHERE map.ProductID = @ProductID AND map.CreateTime BETWEEN '{startDate.Value.ToString("yyyy-MM-dd")} 00:00:00' AND '{endDate.Value.ToString("yyyy-MM-dd")} 23:59:59' ";

                result.Data = db.Database.SqlQuery<SysCatalogProductWeekRanking>(querySql, new SQLiteParameter[] {
                      new SQLiteParameter("@ProductID",productId)
                }).ToList();

                return result;
            }


        }
    }
}

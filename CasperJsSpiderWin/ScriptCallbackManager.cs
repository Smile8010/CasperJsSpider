using CasperJsSpider.Repository;
using CasperJsSpider.Repository.Repository;
using CasperJsSpider.SqlLiteDomain;
using CasperJsSpider.SqlLiteDomain.Model;
using CasperJsSpider.SqlLiteDomain.ViewModel;
using CefSharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpiderWin
{
    /// <summary>
    /// js c#回调类
    /// </summary>
    public class ScriptCallbackManager
    {
        public void GetSysCatalogList(IJavascriptCallback javascriptCallback)
        {
            Task.Factory.StartNew(async () =>
            {
                using (javascriptCallback)
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

                    await javascriptCallback.ExecuteAsync(JsonConvert.SerializeObject(response));
                }
            });
        }

        public void GetCatalogProductRankList(string obj, int start, int limit, IJavascriptCallback javascriptCallback)
        {
            Task.Factory.StartNew(async () =>
            {
                using (javascriptCallback)
                {
                    Result<List<ProductListItemViewModel>> result = new Result<List<ProductListItemViewModel>>(true);
                    ProductListItemQueryViewModel queryEntity = JsonConvert.DeserializeObject<ProductListItemQueryViewModel>(obj);
                    if (queryEntity == null)
                    {
                        result.Success = false;
                        result.Msg = "数据转换失败！";
                    }
                    else
                    {
                        using (DbContext db = DbContextFactory.Context)
                        {
                            //找出mapName
                            var dbSet = db.Set<SysCatalog>();
                            SysCatalog entity = dbSet.First(o => o.ID == queryEntity.CatalogID);
                            List<SQLiteParameter> param = new List<SQLiteParameter>() {
                                new SQLiteParameter("@CatalogID",queryEntity.CatalogID),
                                new SQLiteParameter("@RankLevel",queryEntity.RankNumber),
                                new SQLiteParameter("@StartDate",queryEntity.StartDate),
                                new SQLiteParameter("@EndDate",queryEntity.EndDate)
                            };
                            string sql, fromSql, whereSql, totalSql;

                            fromSql = $"from {entity.CatalogProductTableName} map join tb_Sys_Product p on map.ProductID = p.ID ";
                            whereSql = "where map.CatalogID = @CatalogID ";

                            if (!string.IsNullOrEmpty(queryEntity.Name))
                            {
                                whereSql += $"AND p.Name LIKE @Name ";
                                param.Add(new SQLiteParameter("@Name", $"%{queryEntity.Name}%"));
                            }

                            whereSql += $"AND map.RankLevel = @RankLevel ";
                            whereSql += $"AND map.CreateTime Between @StartDate AND @EndDate ";

                            sql = " select p.Name,p.ImgPath,map.RankTime,map.RankLevel "
                            + fromSql
                            + whereSql
                            + "order by map.CreateTime desc, map.RankLevel asc "
                            + $"limit {limit} offset {start}; ";

                            totalSql = " select count(1) " + fromSql + whereSql;

                            SQLiteParameter[] paramArray = param.ToArray();

                            result.Data = db.Database.SqlQuery<ProductListItemViewModel>(sql, paramArray).ToList();
                            result.Total = db.Database.SqlQuery<int>(totalSql, paramArray).FirstOrDefault();

                        }
                    }
                    await javascriptCallback.ExecuteAsync(JsonConvert.SerializeObject(result));
                    //javascriptCallback.ExecuteAsync(JsonConvert.SerializeObject(result));
                }
            });
        }

        public void GetCatalogProductList(string obj, IJavascriptCallback javascriptCallback)
        {
            Task.Factory.StartNew(async () =>
            {
                using (javascriptCallback)
                {
                    Result<List<CatalogProductListViewModel>> result = new Result<List<CatalogProductListViewModel>>(true);
                    try
                    {
                        ProductListItemQueryViewModel queryEntity = JsonConvert.DeserializeObject<ProductListItemQueryViewModel>(obj);
                        if (queryEntity == null)
                        {
                            result.Success = false;
                            result.Msg = "查询参数转换失败！";
                        }
                        else
                        {
                            using (DbContext db = DbContextFactory.Context)
                            {
                                //找出mapName
                                var dbSet = db.Set<SysCatalog>();
                                SysCatalog entity = dbSet.First(o => o.ID == queryEntity.CatalogID);

                                List<SQLiteParameter> param = new List<SQLiteParameter>() {
                                new SQLiteParameter("@CatalogID",queryEntity.CatalogID)
                            };

                                string sql, whereSql, innerWhereSql;

                                whereSql = " WHERE p.CatalogID = @CatalogID ";
                                if (!string.IsNullOrEmpty(queryEntity.Name))
                                {
                                    whereSql += "AND p.Name LIKE @Name ";
                                    param.Add(new SQLiteParameter("@Name", $"%{queryEntity.Name}%"));
                                }

                                innerWhereSql = " WHERE map.CatalogID = p.CatalogID AND map.ProductID = p.ID ";
                                //AND map.ProductID = p.ID AND map.CreateTime BETWEEN @StartDate AND @EndDate
                                if (queryEntity.StartDate.HasValue && queryEntity.EndDate.HasValue)
                                {
                                    innerWhereSql += " AND map.CreateTime BETWEEN @StartDate AND @EndDate ";
                                    param.Add(new SQLiteParameter("@StartDate", queryEntity.StartDate.Value));
                                    param.Add(new SQLiteParameter("@EndDate", queryEntity.EndDate.Value));
                                }

                                //先找出分类下的产品
                                sql = string.Format(@"SELECT p.Name AS 'ProductName',p.ImgPath AS 'ProductImgPath'
,(SELECT COUNT(1) FROM {0} map {2} ) AS 'TotalCount'
FROM tb_Sys_Product p {1} ", entity.CatalogProductTableName, whereSql, innerWhereSql);

                                List<CatalogProductListViewModel> list = db.Database.SqlQuery<CatalogProductListViewModel>(sql, param.ToArray()).OrderByDescending(o => o.TotalCount).ToList();
                                list.RemoveAll(o => o.TotalCount <= 0);
                                result.Data = list;
                                result.Total = list.Sum(o => o.TotalCount);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.Msg = ex.Message;
                    }

                    await javascriptCallback.ExecuteAsync(JsonConvert.SerializeObject(result));
                }
            });
        }
    }
}

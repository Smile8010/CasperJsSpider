using CasperJsSpider.Repository.Repository;
using CasperJsSpider.SqlLiteDomain;
using CasperJsSpider.SqlLiteDomain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [HttpGet,Route("api/menu")]
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
    }
}

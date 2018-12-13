using CasperJsSpider.SqlLiteDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CasperJsSpiderWin.Controllers
{
    public class HomeController : ApiController
    {
        public Result<object> GetTest()
        {
            //throw new Exception_DG("login id , pwd", "argumets can not be null", 11111, 2222);
            //return Json(new { IsSuccess = true, Msg = "this is get method" });
            return new Result<object>(true,"哈哈");
        }
    }
}

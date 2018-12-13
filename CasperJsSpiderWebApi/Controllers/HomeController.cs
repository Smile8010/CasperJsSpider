using CasperJsSpider.SqlLiteDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CasperJsSpiderWebApi.Controllers
{
   public class HomeController :ApiController
    {
        [Route("api/gettest")]
        public Result<object> GetTest()
        {
            return new Result<object>(true,"Hello world!");
        }
    }
}

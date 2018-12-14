using System;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CasperJsSpider.SqlLiteDomain;
using Microsoft.Owin;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Linq;
using System.IO;
using CasperJsSpider.Comom;

[assembly: OwinStartup(typeof(CasperJsSpiderWebApi.Startup))]

namespace CasperJsSpiderWebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseWebApi(WebApiConfig.OwinWebApiConfiguration(new HttpConfiguration()));

            app.Use<RequestFileMiddleware>();
        }
    }

    /// <summary>
    /// webapi 配置类
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 做为委托提供给System.Web.Http.GlobalConfiguration.Configuration()
        /// 用于webapi以iis为服务器的情况
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            // Web API 路由
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
        /// <summary>
        /// 返回webapi的httpconfiguration配置
        /// 用于webapi应用于owin技术时使用
        /// </summary>
        /// <returns></returns>
        public static HttpConfiguration OwinWebApiConfiguration(HttpConfiguration config)
        {
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Add(new JsonResponseFormatter());
            config.MapHttpAttributeRoutes();//开启属性路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            return config;
        }
    }


    /// <summary>
    /// JsonResponse媒体格式器
    /// </summary>
    public class JsonResponseFormatter : MediaTypeFormatter
    {
        JsonSerializerSettings JsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            ContractResolver = new DefaultContractResolver() { IgnoreSerializableAttribute = true }
        };

        /// <summary>
        /// 初始化
        /// </summary>
        public JsonResponseFormatter()
        {
            this.SupportedMediaTypes.Clear();
            this.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            this.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/json"));
            this.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
        }

        /// <summary>
        /// 是否为可读对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanReadType(Type type)
        {
            return true;
        }

        /// <summary>
        /// 是否为可写对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool CanWriteType(Type type)
        {
            return true;
        }

        /// <summary>
        /// 异步写入值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="writeStream"></param>
        /// <param name="content"></param>
        /// <param name="transportContext"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task WriteToStreamAsync(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext, System.Threading.CancellationToken cancellationToken)
        {
            var objectValue = value;
            if (!GenericEq(type, typeof(Result<>)))
            {
                objectValue = new Result<object>
                {
                    Success = true,
                    Data = value,
                    Msg = ""
                };
            }

            var contentBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objectValue, JsonSerializerSettings));
            return writeStream.WriteAsync(contentBytes, 0, contentBytes.Count(), cancellationToken);
        }

        /// <summary>
        /// 异步读取值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="readStream"></param>
        /// <param name="content"></param>
        /// <param name="formatterLogger"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override System.Threading.Tasks.Task<object> ReadFromStreamAsync(Type type, System.IO.Stream readStream, System.Net.Http.HttpContent content, IFormatterLogger formatterLogger, System.Threading.CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var sr = new System.IO.StreamReader(readStream);
                var objectString = sr.ReadToEnd();

                return JsonConvert.DeserializeObject(objectString, type, JsonSerializerSettings);
            });
        }

        /// <summary>
        /// 判断对象是否相等
        /// </summary>
        /// <param name="type"></param>
        /// <param name="toCompare"></param>
        /// <returns></returns>
        private bool GenericEq(Type type, Type toCompare)
        {
            return type.Namespace == toCompare.Namespace && type.Name == toCompare.Name;
        }
    }

    /// <summary>
    /// 请求文件处理中间件
    /// </summary>
    public class RequestFileMiddleware : OwinMiddleware
    {
        /// <summary>
        /// 构造函数，第一个参数必须为 OwinMiddleware对象 ;第一个参数是固定的，后边还可以添加自定义的其它参数
        /// </summary>
        /// <param name="next">下一个中间件</param>
        public RequestFileMiddleware(OwinMiddleware next)
            : base(next)
        {

        }

        /// <summary>
        /// 处理用户请求的具体方法，该方法是必须的
        /// </summary>
        /// <param name="context">OwinContext对象</param>
        /// <returns></returns>
        public override Task Invoke(IOwinContext context)
        {
            //获取物理文件路径
            var path = GetFilePath(context.Request.Path.Value);
            //验证路径是否存在
            if (File.Exists(path))
            {
                return SetResponse(context, path);
            }

            //不存在返回下一个请求
            return Next.Invoke(context);
        }

        public static string GetFilePath(string relPath)
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory
                , AppConfig.GetValue("SiteDir")
                , relPath.TrimStart('/').Replace('/', '\\'));
        }

        public Task SetResponse(IOwinContext context, string path)
        {
            var perfix = Path.GetExtension(path);
            if (perfix == ".html")
            {
                context.Response.ContentType = "text/html; charset=utf-8";
            }
            else if (perfix == ".js")
            {
                context.Response.ContentType = "application/x-javascript";
            }
            else if (perfix == ".css")
            {
                context.Response.ContentType = "atext/css";
            }
            else if (perfix == ".json")
            {
                context.Response.ContentType = "application/json";
            }
            return context.Response.WriteAsync(File.ReadAllText(path));
        }

    }
}

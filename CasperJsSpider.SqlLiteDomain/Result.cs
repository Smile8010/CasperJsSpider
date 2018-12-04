using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CasperJsSpider.SqlLiteDomain
{
    /// <summary>
    /// 返回结果类（用于json序列化）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result<T>
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Result() : this(false, "默认结果！", default(T)) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">结果状态</param>
        public Result(bool success) : this(success, success ? "操作成功！" : "操作失败，请稍后再试！", default(T)) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">结果状态</param>
        /// <param name="msg">提示消息</param>
        public Result(bool success, string msg) : this(success, msg, default(T)) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">结果状态</param>
        /// <param name="level">消息等级</param>
        public Result(bool success, ResultLevel level = 0) : this(success, string.Empty, default(T), level) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">结果状态</param>
        /// <param name="msg">提示消息</param>
        /// <param name="level">消息等级</param>
        public Result(bool success, string msg, ResultLevel level = 0) : this(success, msg, default(T), level) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data">结果</param>
        public Result(T data)
            : this(
                data == null || string.IsNullOrWhiteSpace(data.ToString()) ? false : true,
                data == null || string.IsNullOrWhiteSpace(data.ToString()) ? "操作失败，请稍后再试！" : "操作成功！",
                data)
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="success">结果状态</param>
        /// <param name="msg">提示消息</param>
        /// <param name="data">结果</param>
        public Result(bool success, string msg, T data, ResultLevel level = 0)
        {
            Success = success;
            Msg = msg;
            Data = data;
            Level = level;
            if (string.IsNullOrWhiteSpace(Msg))
            {
                switch (Level)
                {
                    case ResultLevel.ERROR: Msg = "错误提示：操作出错，请稍后再试！"; break;
                    case ResultLevel.WARNING: Msg = "警告提示：操作失败，请稍后再试！"; break;
                    case ResultLevel.NOLOGIN: Msg = "您未登录或登录已超时，请重新登录！"; break;
                    case ResultLevel.NOFOLLOW: Msg = "您未关注该公众号，请关注后重试！"; break;
                    default: Msg = Success ? "提示信息，操作成功！" : "提示信息，操作失败，请稍后再试！"; break;
                }
            }
            if (Success && Data != null)
            {
                DataTotal = Data.GetType().GetInterface("ICollection") != null ? (Data as ICollection).Count : 1;
            }
            else
            {
                DataTotal = 0;
            }

        }

        /// <summary>
        /// 结果状态
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 消息等级
        /// </summary>
        public ResultLevel Level { get; set; }

        /// <summary>
        /// 返回结果的实际个数
        /// </summary>
        public int DataTotal { get; set; }

        /// <summary>
        /// 满足条件的个数，分页时候赋值
        /// </summary>
        public long Total { get; set; }


        /// <summary>
        /// 通过异常构造Result对象
        /// </summary>
        /// <param name="ex">异常</param>
        /// <returns></returns>
        public static Result<T> CreateByException(System.Exception ex)
        {
            return new Result<T>(false, ex.Message, default(T), ex.Message.Contains("未登录") ? ResultLevel.NOLOGIN : ResultLevel.ERROR);
        }
    }

    /// <summary>
    /// 消息等级
    /// </summary>
    public enum ResultLevel
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        INFO = 0,

        /// <summary>
        /// 警告信息
        /// </summary>
        WARNING = 1,

        /// <summary>
        /// 错误信息
        /// </summary>
        ERROR = 2,

        /// <summary>
        /// 未登录
        /// </summary>
        NOLOGIN = 3,

        /// <summary>
        /// 未关注
        /// </summary>
        NOFOLLOW = 4,

        /// <summary>
        /// 重定向消息
        /// </summary>
        REDIRECT = 5,

        /// <summary>
        /// 刷新界面
        /// </summary>
        REFRESHPAGE = 6,

        /// <summary>
        /// 无权访问
        /// </summary>
        UNAUTHORIZE = 7
    }
}

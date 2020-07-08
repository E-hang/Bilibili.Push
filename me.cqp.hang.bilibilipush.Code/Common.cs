using Native.Sdk.Cqp;
using Native.Sdk.Cqp.Model;

namespace me.cqp.hang.bilibilipush.Code
{
    public class Common
    {
        /// <summary>
		/// 获取当前 App 使用的 酷Q Api 接口实例
		/// </summary>
		public static CQApi CQApi { get; private set; }

        /// <summary>
        /// 获取当前 App 使用的 酷Q Log 接口实例
        /// </summary>
        public static CQLog CQLog { get; private set; }
    }
}

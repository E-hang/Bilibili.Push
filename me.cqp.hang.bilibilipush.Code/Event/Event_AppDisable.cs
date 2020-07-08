using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Tool.Log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.hang.bilibilipush.Code.Event
{
    public class Event_AppDisable : IAppDisable
    {
        /// <summary>
        /// 处理 酷Q 的插件禁用事件回调
        /// </summary>
        /// <param name="sender">事件的触发对象</param>
        /// <param name="e">事件的附加参数</param>
        public void AppDisable(object sender, CQAppDisableEventArgs e)
        {
            Event_AppEnable.timer.Stop();
            Log4Helper.Info("B站特别关注小插件已停用");
        }
    }
}

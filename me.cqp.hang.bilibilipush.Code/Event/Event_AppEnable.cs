using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Tool.Log4net;
using System.Data.SQLite;
using System.Threading;
using System.Timers;

namespace me.cqp.hang.bilibilipush.Code.Event
{
    /// <summary>
    /// Type=1003 应用被启用, 事件实现
    /// </summary>
    public class Event_AppEnable : IAppEnable
    {
        public static System.Timers.Timer timer = new System.Timers.Timer();
        public static CQAppEnableEventArgs c;
        public static Thread t2 = new Thread(demo);
        private static bool B_Timer = true;

        /// <summary>
        /// 处理 酷Q 的插件启动事件回调
        /// </summary>
        /// <param name="sender">事件的触发对象</param>
        /// <param name="e">事件的附加参数</param>
        public void AppEnable(object sender, CQAppEnableEventArgs e)
        {
            // 检查并更新数据库
            Event_CheckDatabase.CheckDatabase();

            //创建一个推断的委托，调用计时器的方法。
            timer.Interval = 1000 * 3;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(CustomTimer);
            //设置是执行一次（false）还是一直执行(true)；
            timer.AutoReset = true;
            //是否执行System.Timers.Timer.Elapsed事件；
            timer.Enabled = true;
            //计时器启动
            //timer.Start();

            t2.IsBackground = true;
            t2.Start(e);
            Log4Helper.Info("B站特别关注小插件已加载");
        }


        public static void demo(object e)
        {
            long l = 168628068;
            c = (CQAppEnableEventArgs)e;
            c.CQApi.SendGroupMessage(l, "测试");
            while (true)
            {
                Log4Helper.Info("线程：" + t2.Name + " " + "ID：" + t2.ManagedThreadId + "正在运行");
                // 暂停1秒
                Thread.Sleep(1000);
                Event_GetDynamicInform.GetDynamicInform(c);
            }
        }

        public static void CustomTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            if (B_Timer)
            {
                B_Timer = false;
                Log4Helper.Info("B_Timer:" + B_Timer.ToString() + "\nCustomTimer开始");
                //Event_DynamicInform.DynamicInform();

            }
            B_Timer = true;
            Log4Helper.Info("B_Timer:" + B_Timer.ToString() + "\nCustomTimer结束");
        }
    }
}

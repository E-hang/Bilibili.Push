using me.cqp.hang.bilibilipush.Code.Event;
using Native.Sdk.Cqp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace Native.Core
{
    /// <summary>
    /// 酷Q应用主入口类
    /// </summary>
    public class CQMain
    {
        /// <summary>
        /// 在应用被加载时将调用此方法进行事件注册, 请在此方法里向 <see cref="IUnityContainer"/> 容器中注册需要使用的事件
        /// </summary>
        /// <param name="container">用于注册的 IOC 容器 </param>
        public static void Register(IUnityContainer unityContainer)
        {
            // 注入 Type=1001 的回调
            //unityContainer.RegisterType<ICQStartup, Event_CQStartup>("酷Q启动事件");
            // 注入 Type=1002 的回调
            //unityContainer.RegisterType<ICQExit, Event_CQExit>("酷Q关闭事件");
            // 注入 Type=1003 的回调
            unityContainer.RegisterType<IAppEnable, Event_AppEnable>("应用已被启用");
            // 注入 Type=1004 的回调
            unityContainer.RegisterType<IAppDisable, Event_AppDisable>("应用将被停用");
            // 将实现了接口的类注入到容器中, 并且注入的名称就为 Json 中使用的 "name" 字段
            unityContainer.RegisterType<IGroupMessage, Event_GroupMessage>("群消息处理");
        }
    }
}

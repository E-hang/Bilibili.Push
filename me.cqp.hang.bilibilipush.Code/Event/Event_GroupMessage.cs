﻿using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.hang.bilibilipush.Code.Event
{
    public class Event_GroupMessage : IGroupMessage
    {
        public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {
            // 获取 At 某人对象
            //CQCode cqat = e.FromQQ.CQCode_At();
            // 往来源群发送一条群消息, 下列对象会合并成一个字符串发送
            //e.FromGroup.SendGroupMessage(cqat, " 您发送了一条消息: ", e.Message);
            // 设置该属性, 表示阻塞本条消息, 该属性会在方法结束后传递给酷Q
            //e.Handler = true;
        }
    }
}
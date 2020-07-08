using Native.Sdk.Cqp;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Tool.Http;
using Native.Tool.Log4net;
using Native.Tool.SQLite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace me.cqp.hang.bilibilipush.Code.Event
{
    public class Event_GetDynamicInform
    {
        public static string New_DynamicId = "";
        public static string Old_DynamicId = "";

        // api地址
        public static string strUrl1 = @"http://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/space_history?host_uid=";

        public static string returnData = "";
        public static string g_msg = "";
        public static CQAppEnableEventArgs c;

        public static void GetDynamicInform(object e)
        {
            Log4Helper.Info("获取动态信息GetDynamicInform开始");

            c = (CQAppEnableEventArgs)e;

            // 获取数据库里关注的UP的UID
            string[] arrUID = SQLiteHelper.ReadFullTable("UP", SQLiteHelper.CountRows("UP"));

            if (arrUID.Length != 0)
            {
                // 循环各个UID
                for (int i = 0; i < arrUID.Length; i++)
                {
                    // 循环开始，进行初始化
                    returnData = "";

                    // 查询数据库里当前UID对应的动态ID
                    Old_DynamicId = SQLiteHelper.ReadValue("UP", "Dynamic_Id_Str", "UID", arrUID[i].ToString());
                    Log4Helper.Info(arrUID[i] + ":" + Old_DynamicId);

                    // 访问API，获取arrUID[i]对应的数据
                    returnData = HttpTool.HttpGet(string.Concat(strUrl1, arrUID[i]));
                    RootObject rb = JsonConvert.DeserializeObject<RootObject>(returnData);

                    if (rb.data == null)
                    {
                        Log4Helper.Warn("获取动态信息失败，跳过本次");
                    }
                    else
                    {
                        if (rb.data.cards[0].desc.dynamic_id_str != null)
                        {
                            try
                            {
                                New_DynamicId = rb.data.cards[0].desc.dynamic_id_str;
                            }
                            catch (ArgumentOutOfRangeException)
                            {
                                System.Diagnostics.Debug.WriteLine("语句：strDynamicId = rb.data.cards[0].desc.dynamic_id_str，发生异常");
                                Log4Helper.Debug("语句：strDynamicId = rb.data.cards[0].desc.dynamic_id_str，发生异常");
                                throw;
                            }

                            //如果数据库没有值，接口获取到值
                            if (Old_DynamicId == "" && New_DynamicId != "")
                            {
                                Old_DynamicId = New_DynamicId;
                                SQLiteHelper.UpdateValues("UP", "Dynamic_Id_Str", New_DynamicId, "UID", "=", arrUID[i]);
                                Log4Helper.Info("插入UID：" + arrUID[i] + "的动态ID:" + New_DynamicId);
                            }

                            //如果接口获取到的值 不等于 数据库里的值
                            else if (New_DynamicId != Old_DynamicId)
                            {
                                // 把New_DynamicId更新进数据库中
                                SQLiteHelper.UpdateValues("UP", "Dynamic_Id_Str", New_DynamicId, "UID", "=", arrUID[i]);
                                g_msg = "更新了！最新动态地址：http://t.bilibili.com/" + New_DynamicId;
                                Log4Helper.Debug(g_msg);

                                // 获取UID对应的昵称
                                string UPName = SQLiteHelper.ReadValue("UP", "Name", "UID", arrUID[i]);
                                Log4Helper.Info("UPName为：" + UPName);

                                // 获取群号
                                string followGroup = SQLiteHelper.ReadValue("UP", "Follow_Group", "UID", arrUID[i]);
                                Log4Helper.Info("followGroup为：" + followGroup);

                                // 分割群号，存入数组中
                                string[] strFollowGroup = followGroup.Split('/');

                                // 循环通知关注该UID的各个群
                                for (int j = 0; j < strFollowGroup.Length; j++)
                                {
                                    Log4Helper.Info("这次通知的群是：" + strFollowGroup[j]);
                                    //CQApi CQApi;
                                    //SendGroupMessage(long.Parse(strFollowGroup[i]), "您关注的UP：" + UPName + "有新的动态！\n地址是：" + g_msg);
                                    c.CQApi.SendGroupMessage(long.Parse(strFollowGroup[i]),
                                                             "您关注的UP：" + UPName + "有新的动态！\n最新动态地址：http://t.bilibili.com/" + New_DynamicId);
                                }


                                g_msg = "新动态ID是：" + New_DynamicId + "；旧动态ID是：" + Old_DynamicId;
                                Log4Helper.Debug(g_msg);
                                if (SQLiteHelper.ReadValue("UP", "Dynamic_Id_Str", "UID", arrUID[i].ToString()) == New_DynamicId)
                                {
                                    Log4Helper.Debug("插入Dynamic_Id_Str成功");
                                }
                                else
                                {
                                    Log4Helper.Error("插入Dynamic_Id_Str失败");
                                }
                            }

                            //如果接口获取到的值 等于 数据库里的值
                            else if (New_DynamicId == Old_DynamicId)
                            {
                                g_msg = "未更新，当前动态ID为：" + Old_DynamicId;
                                Log4Helper.Info(g_msg);
                            }
                        }
                        else
                        {
                            g_msg = "当前up：" + rb.data.cards[0].desc.user_profile.info.uname + "没有动态";
                            Log4Helper.Debug(g_msg);
                        }
                    }
                }

            }

            Log4Helper.Info("获取动态信息GetDynamicInform开始结束");
        }
    }

    public class Dynamic
    {
        public string New_DynamicId { get; set; }
        public string Old_DynamicId { get; set; }
        public string Is_new { get; set; }
        public string Up_Name { get; set; }
    }

    public class Uids
    {
    }

    public class Attentions
    {
        public List<Uids> uids { get; set; }
    }

    public class Info
    {
        public string uid { get; set; }
        public string uname { get; set; }
        public string face { get; set; }
    }

    public class Official_verify
    {
        public string type { get; set; }
        public string desc { get; set; }
    }

    public class Card
    {
        public Official_verify official_verify { get; set; }
    }

    public class Label
    {
        public string path { get; set; }
    }

    public class Vip
    {
        public string vipType { get; set; }
        public string vipDueDate { get; set; }
        public string dueRemark { get; set; }
        public string accessStatus { get; set; }
        public string vipStatus { get; set; }
        public string vipStatusWarn { get; set; }
        public string themeType { get; set; }
        public Label label { get; set; }
    }

    public class Pendant
    {
        public string pid { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public string expire { get; set; }
    }

    public class Level_info
    {
        public string current_level { get; set; }
        public string current_min { get; set; }
        public string current_exp { get; set; }
        public string next_exp { get; set; }
    }

    public class User_profile
    {
        public Info info { get; set; }
        public Card card { get; set; }
        public Vip vip { get; set; }
        public Pendant pendant { get; set; }
        public string rank { get; set; }
        public string sign { get; set; }
        public Level_info level_info { get; set; }
    }

    public class Desc
    {
        public string uid { get; set; }
        public string type { get; set; }
        public string rid { get; set; }
        public string acl { get; set; }
        public string view { get; set; }
        public string repost { get; set; }
        public string comment { get; set; }
        public string like { get; set; }
        public string is_liked { get; set; }
        public string dynamic_id { get; set; }
        public string timestamp { get; set; }
        public string pre_dy_id { get; set; }
        public string orig_dy_id { get; set; }
        public string orig_type { get; set; }
        public User_profile user_profile { get; set; }
        public string uid_type { get; set; }
        public string stype { get; set; }
        public string r_type { get; set; }
        public string inner_id { get; set; }
        public string status { get; set; }
        public string dynamic_id_str { get; set; }
    }

    public class Topic_details
    {
        public string topic_id { get; set; }
        public string topic_name { get; set; }
        public string is_activity { get; set; }
        public string topic_link { get; set; }
    }

    public class Topic_info
    {
        public List<Topic_details> topic_details { get; set; }
    }

    public class Origin
    {
        public Topic_info topic_info { get; set; }
    }

    public class Display
    {
        public Origin origin { get; set; }
    }

    public class Cards
    {
        public Desc desc { get; set; }
        public string card { get; set; }
        public Display display { get; set; }
    }

    public class Data
    {
        public string has_more { get; set; }
        public Attentions attentions { get; set; }
        public List<Cards> cards { get; set; }
        public string _gt_ { get; set; }
    }

    public class RootObject
    {
        public string code { get; set; }
        public string msg { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }
}

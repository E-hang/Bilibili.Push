using me.cqp.hang.bilibilipush.Code.Json;
using Native.Tool.Http;
using Native.Tool.Log4net;
using Native.Tool.SQLite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.hang.bilibilipush.Code.Event
{
    class Event_GetVideoInformation
    {
        //api地址
        static string strUrl1 = @"http://space.bilibili.com/ajax/member/getSubmitVideos?mid=";
        static string strUrl2 = "&pagesize=1&page=1";
        // https://api.bilibili.com/x/space/arc/search?mid=163683&ps=1&tid=0&pn=1&keyword=&order=pubdate&jsonp=jsonp
        // https://api.bilibili.com/x/space/arc/search?mid=163683&ps=1&tid=0&pn=1

        static string returnData = "";
        static string Old_Aid = "";
        static string New_Aid = "";

        public static void GetVideoInformation()
        {
            Log4Helper.Info("获取投稿信息GetVideoInformation开始");

            // 获取数据库里关注的UP的UID
            string[] arrUID = SQLiteHelper.ReadFullTable("UP", SQLiteHelper.CountRows("UP"));

            if (arrUID.Length != 0)
            {
                // 循环各个UID
                for (int i = 0; i < arrUID.Length; i++)
                {
                    // 循环开始，进行初始化
                    returnData = "";

                    // 查询数据库里当前UID对应的Aid
                    Old_Aid = SQLiteHelper.ReadValue("UP", "Aid", "UID", arrUID[i]);

                    Log4Helper.Info("数据库中" + arrUID[i] + "的Aid是:" + Old_Aid);

                    returnData = HttpTool.HttpGet(string.Concat(strUrl1, arrUID[i], strUrl2));
                    Json_VideoInformation.RootObject rb = JsonConvert.DeserializeObject<Json_VideoInformation.RootObject>(returnData);

                    if (rb.data == null)
                    {
                        Log4Helper.Warn("获取投稿信息失败，跳过" + arrUID[i]);
                    }
                    else
                    {
                        // 如果返回的Json显示有投稿视频
                        if (rb.data.count != "0" || rb.data.count != null)
                        {
                            if (rb.data.vlist[0].aid != null)
                            {
                                try
                                {
                                    New_Aid = rb.data.vlist[0].aid;
                                    Log4Helper.Warn("Old_Aid：" + Old_Aid + "\n" + " New_Aid：" + New_Aid);
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    System.Diagnostics.Debug.WriteLine("语句：New_Aid = rb.data.vlist[0].aid;，发生异常");
                                    throw;
                                }
                                // 如果数据库没有值，接口获取到值
                                if (Old_Aid == "" && New_Aid != "")
                                {
                                    // 直接把获取到的值更新进数据库
                                    SQLiteHelper.UpdateValues("UP", "Aid", New_Aid, "UID", "=", arrUID[i]);
                                }
                                // 如果数据库里的值 不等于 接口获取到的值
                                else if (New_Aid != Old_Aid)
                                {

                                }
                            }
                        }
                        else
                        {
                            Log4Helper.Info(arrUID[i] + "，该ID没有视频投稿。");
                        }
                    }
                }
            }
            Log4Helper.Info("获取投稿信息GetVideoInformation结束");
        }
    }
}

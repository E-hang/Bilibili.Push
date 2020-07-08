using me.cqp.hang.bilibilipush.Code.Json;
using Native.Sdk.Cqp.EventArgs;
using Native.Sdk.Cqp.Interface;
using Native.Sdk.Cqp.Model;
using Native.Tool;
using Native.Tool.Http;
using Native.Tool.Log4net;
using Native.Tool.SQLite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace me.cqp.hang.bilibilipush.Code.Event
{
    public class Event_GroupMessage : IGroupMessage
    {
        #region 变量
        static string[] instructionGroup = { ".查看命令", ".增加关注:UID", ".取消关注:UID", ".开启管理员权限", ".关闭管理员权限" };
        static string msg = "";
        static string UID;

        static string returnData = "";
        static string urlUpInformation = @"https://api.bilibili.com/x/space/acc/info?mid=";
        static string followGroup = "";
        static string[] strfollowGroup;
        static bool isFollow;
        #endregion

        public void GroupMessage(object sender, CQGroupMessageEventArgs e)
        {
            if (Regex.IsMatch(e.Message, @"^+\.+[\u4e00-\u9fa5]"))
            {
                Log4Helper.Info("收到消息：" + e.Message);

                #region 命令：.查看命令相关代码
                if (e.Message == ".查看命令")
                {
                    Log4Helper.Info("触发命令：" + e.Message);
                    msg = "当前可用命令：" + "\n";
                    for (int i = 0; i < instructionGroup.Length; i++)
                    {
                        msg = msg + instructionGroup[i] + "\n";
                    }
                    e.CQApi.SendGroupMessage(e.FromGroup, msg);
                }
                #endregion

                #region 命令：.增加关注相关代码
                if (Regex.IsMatch(e.Message, ".增加关注:" + @"\d"))
                {
                    Log4Helper.Info("触发命令：" + e.Message);

                    // 初始化
                    returnData = "";

                    // 获取命令里的UID
                    UID = Regex.Replace(e.Message, @"[^0-9]+", "");
                    e.CQApi.SendGroupMessage(e.FromGroup, "收到消息：" + e.Message + "\n" + "UID是：" + UID);

                    // 访问API，并获取返回的json
                    returnData = HttpTool.HttpGet(string.Concat(urlUpInformation, UID));
                    Json_UpInformation.RootObject rb = JsonConvert.DeserializeObject<Json_UpInformation.RootObject>(returnData);

                    if (rb.data == null)
                    {
                        e.CQApi.SendGroupMessage(e.FromGroup, "此UID无效，请检查UID");
                    }
                    else
                    {
                        followGroup = SQLiteHelper.ReadValue("UP", "Follow_Group", "UID", UID);

                        // 如果数据库没有输入的UID的记录，则新增一条该UID的记录。
                        if (SQLiteHelper.ReadValue("UP", "UID", "UID", UID) == null || SQLiteHelper.ReadValue("UP", "UID", "UID", UID) == "")
                        {
                            SQLiteHelper.InsertValues("UP", new string[] { UID, rb.data.name, "", "0", "", "", e.FromGroup, "", "2019/1/1 0:00:00", e.FromGroup, "", e.FromGroup, "", e.FromGroup, "" });
                            e.CQApi.SendGroupMessage(e.FromGroup, "已成功关注：" + rb.data.name);
                        }
                        else
                        {
                            // 如果数据库有输入的UID的记录，但Follow_Group为空时，直接更新Follow_Group字段
                            if (followGroup == "" || followGroup == null)
                            {
                                Log4Helper.Info("Follow_Group为空，直接更新Follow_Group字段");

                                SQLiteHelper.UpdateValues("UP",
                                                          new string[] { "Follow_Group", "Dynamic_Group", "Video_Group", "Live_Group" },
                                                          new string[] { e.FromGroup, e.FromGroup, e.FromGroup, e.FromGroup },
                                                          "UID",
                                                          UID,
                                                          "=");
                                e.CQApi.SendGroupMessage(e.FromGroup, "已成功关注：" + rb.data.name);
                            }
                            else
                            {
                                Log4Helper.Info("Follow_Group不为空，检查触发的群是否已关注输入的UID");

                                strfollowGroup = followGroup.Split('/');

                                // 用于判断是否关注，默认为未关注
                                isFollow = false;

                                // 检查触发的群是否已关注输入的UID
                                for (int i = 0; i < strfollowGroup.Length; i++)
                                {
                                    if (strfollowGroup[i] == e.FromGroup)
                                    {
                                        // 检测到相同群号时，更改为已关注
                                        isFollow = true;
                                        // 提示已关注
                                        e.CQApi.SendGroupMessage(e.FromGroup, "本群已关注：" + rb.data.name + "，请勿重复关注。");
                                        break;
                                    }
                                }
                                // 如果未关注
                                if (isFollow == false)
                                {
                                    // 在UP表各个字段加上群号
                                    Tool.AddGroup("Follow_Group", e.FromGroup.ToString(), UID);
                                    Tool.AddGroup("Dynamic_Group", e.FromGroup.ToString(), UID);
                                    Tool.AddGroup("Video_Group", e.FromGroup.ToString(), UID);
                                    Tool.AddGroup("Live_Group", e.FromGroup.ToString(), UID);

                                    // 加上群号后发送通知
                                    e.CQApi.SendGroupMessage(e.FromGroup, "已成功关注：" + rb.data.name);

                                    // 直接在Follow_Group后加入触发得群号
                                    //followGroup = followGroup + "/" + e.FromGroup;
                                    //strfollowGroup = followGroup.Split('/');
                                    //SQLiteHelper.UpdateValues("UP",
                                    //                          new string[] { "Follow_Group", "Dynamic_Group", "Video_Group", "Live_Group" },
                                    //                          new string[] { followGroup, followGroup, followGroup, followGroup },
                                    //                          "UID",
                                    //                          UID,
                                    //                          "=");
                                    //e.CQApi.SendGroupMessage(e.FromGroup, "已成功关注：" + rb.data.name);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 命令：.取消关注相关代码
                if (Regex.IsMatch(e.Message, ".取消关注:" + @"\d"))
                {
                    Log4Helper.Info("触发命令：" + e.Message);

                    // 获取命令里的UID
                    UID = Regex.Replace(e.Message, @"[^0-9]+", "");
                    e.CQApi.SendGroupMessage(e.FromGroup, "收到消息：" + e.Message + "\n" + "UID是：" + UID);

                    // 访问API，并获取返回的json
                    returnData = HttpTool.HttpGet(string.Concat(urlUpInformation, UID));
                    Json_UpInformation.RootObject rb = JsonConvert.DeserializeObject<Json_UpInformation.RootObject>(returnData);

                    if (rb.data == null)
                    {
                        e.CQApi.SendGroupMessage(e.FromGroup, "此UID无效，请检查UID");
                    }
                    else
                    {
                        // 如果数据库没有输入的UID的记录，直接返回消息，isFollow为false
                        if (SQLiteHelper.ReadValue("UP", "UID", "UID", UID) == null || SQLiteHelper.ReadValue("UP", "UID", "UID", UID) == "")
                        {
                            e.CQApi.SendGroupMessage(e.FromGroup, "本群并未关注：" + rb.data.name);
                        }
                        else
                        {
                            // 获取数据库里关注该UID的followGroup字段的数据
                            followGroup = SQLiteHelper.ReadValue("UP", "Follow_Group", "UID", UID);

                            // 如果数据库有输入的UID的记录，但Follow_Group为空时,isFollow为false
                            if (followGroup == "" || followGroup == null)
                            {
                                Log4Helper.Info("Follow_Group为空，直接返回提示信息");

                                e.CQApi.SendGroupMessage(e.FromGroup, "本群并未关注：" + rb.data.name);
                            }
                            else
                            {
                                Log4Helper.Info("Follow_Group不为空，检查触发的群是否已关注输入的UID");

                                // 如果数据库里关注该UP的群只有触发命令的群时，直接把相应字段置空，并返回消息
                                if (followGroup == e.FromGroup)
                                {
                                    SQLiteHelper.UpdateValues("UP",
                                                                  new string[] { "Follow_Group", "Dynamic_Group", "Video_Group", "Live_Group" },
                                                                  new string[] { "", "", "", "" },
                                                                  "UID",
                                                                  UID,
                                                                  "=");
                                    e.CQApi.SendGroupMessage(e.FromGroup, "已成功取消关注：" + rb.data.name);
                                }
                                // 如果关注该UID的Follow_Group不止一个时
                                else
                                {
                                    strfollowGroup = followGroup.Split('/');
                                    List<string> listFollowGroup = followGroup.Split('/').ToList();

                                    // 检查触发的群是否已关注输入的UID
                                    for (int i = listFollowGroup.Count - 1; i >= 0; i--)
                                    {
                                        if (listFollowGroup[i] == e.FromGroup)
                                        {
                                            //// 检测到相同群号时，更改为已关注
                                            //isFollow = true;

                                            //// 移除触发的群号
                                            //listFollowGroup.Remove(listFollowGroup[i]);

                                            //// 以“/”为间隔，组合List中的各个群号
                                            //followGroup = string.Join("/", listFollowGroup.ToArray());

                                            //// 把去除触发群号后的Follow_Group更新到数据库中
                                            //SQLiteHelper.UpdateValues("UP",
                                            //                      new string[] { "Follow_Group" },
                                            //                      new string[] { followGroup },
                                            //                      "UID",
                                            //                      UID,
                                            //                      "=");

                                            //检查各个字段的数据，并移字段内除触发的群号
                                            Tool.RemoveGroup("Follow_Group", e.FromGroup.ToString(), UID);
                                            Tool.RemoveGroup("Dynamic_Group", e.FromGroup.ToString(), UID);
                                            Tool.RemoveGroup("Video_Group", e.FromGroup.ToString(), UID);
                                            Tool.RemoveGroup("Live_Group", e.FromGroup.ToString(), UID);

                                            // 发送取消成功的消息
                                            e.CQApi.SendGroupMessage(e.FromGroup, "操作成功，已取消关注：" + rb.data.name + "。");
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
        }
    }
}

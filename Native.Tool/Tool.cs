using Native.Tool.Log4net;
using Native.Tool.SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Tool
{
    public class Tool
    {
        /// <summary>
        /// 给UP表相应字段直接增加群号
        /// </summary>
        /// <param name="colName">字段名</param>
        /// <param name="fromGroup">触发的群号</param>
        /// <param name="UID">触发群输入的UID</param>
        public static void AddGroup(string colName, string fromGroup, string UID)
        {
            Log4Helper.Info("调用AddGroup开始");

            // 获取UP里对应字段的数据
            string strGroup = SQLiteHelper.ReadValue("UP", colName, "UID", UID);

            if (strGroup == "" || strGroup == null)
            {
                Log4Helper.Info(colName + "为空，直接更新" + colName + "字段");

                // 更新进数据库
                SQLiteHelper.UpdateValues("UP",
                                          new string[] { colName },
                                          new string[] { fromGroup },
                                          "UID",
                                          UID,
                                          "=");
            }
            else
            {
                // 直接在要修改得字段后加入触发得群号
                strGroup = strGroup + "/" + fromGroup;

                // 更新进数据库
                SQLiteHelper.UpdateValues("UP",
                                          new string[] { colName },
                                          new string[] { strGroup },
                                          "UID",
                                          UID,
                                          "=");
            }

            Log4Helper.Info("调用AddGroup结束");
        }

        public static void RemoveGroup(string colName, string fromGroup, string UID)
        {
            Log4Helper.Info("调用RemoveGroup开始");

            string strGroup = SQLiteHelper.ReadValue("UP", colName, "UID", UID);

            List<string> listFollowGroup = strGroup.Split('/').ToList();

            for (int i = listFollowGroup.Count - 1; i >= 0; i--)
            {
                if (listFollowGroup[i] == fromGroup)
                {
                    // 移除触发的群号
                    listFollowGroup.Remove(listFollowGroup[i]);

                    // 以“/”为间隔，组合List中的各个群号
                    strGroup = string.Join("/", listFollowGroup.ToArray());

                    // 把去除触发群号后的Follow_Group更新到数据库中
                    SQLiteHelper.UpdateValues("UP",
                                          new string[] { colName },
                                          new string[] { strGroup },
                                          "UID",
                                          UID,
                                          "=");

                    Log4Helper.Info("调用RemoveGroup结束");
                }
            }
        }
    }
}

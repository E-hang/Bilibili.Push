using Native.Tool.Log4net;
using Native.Tool.SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.hang.bilibilipush.Code.Event
{
    public class Event_CheckDatabase
    {
        public static void CheckDatabase()
        {
            string version;
            string queryString;

            // 如果数据库文件BilibiliPush.db不存在则创建创建数据库文件
            if (File.Exists("BilibiliPush.db") == false)
            {
                SQLiteHelper.NewDbFile("BilibiliPush.db");
            }

            // 如果表DB_VERSION不存在则创建
            if (SQLiteHelper.CheckTable("DB_VERSION") == false)
            {
                queryString = @"CREATE TABLE DB_VERSION (
                                    Version VARCHAR (1024) PRIMARY KEY
                                    UNIQUE
                                    );
                                    ";
                SQLiteHelper.ExecuteQuery(queryString);

                // 并插入数据
                queryString = @"INSERT INTO DB_VERSION ( VERSION ) VALUES (  '2.0.0' );";
                SQLiteHelper.ExecuteQuery(queryString);
            }

            version = SQLiteHelper.ReadValue("DB_VERSION", "VERSION", "1", "1");

            if (version == "2.0.0")
            {
                #region 注释
                /*
                 * 创建UP表，记录UP相关信息
                 * UID：UP的UID
                 * Name：UP的昵称
                 * Aid：UP的最新视频的AV号
                 * Dynamic_Id_Str：UP最新动态的ID
                 * Follow_Group：关注UP的群号
                 * Follow_Fans：关注该UP的粉丝
                 */
                #endregion
                queryString = @"CREATE TABLE UP (
                                    UID              VARCHAR (256)  PRIMARY KEY
                                                                    UNIQUE
                                                                    NOT NULL,
                                    Name             VARCHAR (253),
                                    Aid              VARCHAR (256),
                                    Live_Status      VARCHAR (2)    NOT NULL
                                                                    DEFAULT (0),
                                    Dynamic_Id_Str   VARCHAR (256),
                                    Old_Dynamic_Id   VARCHAR (256),
                                    Follow_Group     VARCHAR (1024),
                                    Follow_Fans      VARCHAR (1024),
                                    Live_Notice_Time VARCHAR (32),
                                    Dynamic_Group    VARCHAR (1024),
                                    Dynamic_Fans     VARCHAR (1024),
                                    Video_Group      VARCHAR (1024),
                                    Video_Fans       VARCHAR (1024),
                                    Live_Group       VARCHAR (1024),
                                    Live_Fans        VARCHAR (1024) 
                                );";
                SQLiteHelper.ExecuteQuery(queryString);
                Log4Helper.Info("UP表已创建");

                // 创建VIDEO_INFORMATION表，记录视频的AV号，BV号，创建的时间戳
                queryString = @"CREATE TABLE VIDEO_INFORMATION (
                                    Aid     VARCHAR (256)  UNIQUE,
                                    Bvid    VARCHAR (256),
                                    Created VARCHAR (1024) 
                                );";
                SQLiteHelper.ExecuteQuery(queryString);
                Log4Helper.Info("VIDEO_INFORMATION表已创建");

                queryString = @"CREATE TABLE DYNAMIC_INFORMATION (
                                    Dynamic_Id_Str VARCHAR (256)  PRIMARY KEY
                                                                  NOT NULL,
                                    Timestamp      VARCHAR (1024) 
                                );";
                SQLiteHelper.ExecuteQuery(queryString);
                Log4Helper.Info("DYNAMIC_INFORMATION表已创建");

                #region 注释
                /*
                 * 创建GROUP_FUNCTION表，记录各个群的功能开关情况
                 * Group_Number：群号
                 * Admin_Authority：Y/N，是否开启管理员权限
                 */
                #endregion
                queryString = @"CREATE TABLE GROUP_FUNCTION (
                                    Group_Number    VARCHAR (36) PRIMARY KEY
                                                                 UNIQUE,
                                    Admin_Authority VARCHAR (4) 
                                );";
                SQLiteHelper.ExecuteQuery(queryString);
                Log4Helper.Info("GROUP_FUNCTION表已创建");

                // 更新数据库版本号
                SQLiteHelper.UpdateValues("DB_VERSION", "VERSION", "2.0.1", "VERSION", "=", version);
                version = SQLiteHelper.ReadValue("DB_VERSION", "VERSION", "1", "1");
                Log4Helper.Info("数据库版本号已更新为" + version);
            }
        }
    }
}


using Native.Tool.Log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Tool.SQLite
{
    public class SQLiteHelper
    {
        /// <summary>
        /// 数据库连接定义
        /// </summary>
        public static SQLiteConnection dbConnection;

        ///// <summary>
        ///// SQL命令定义
        ///// </summary>
        //private static SQLiteCommand dbCommand;

        /// <summary>
        /// 数据读取定义
        /// </summary>
        private static SQLiteDataReader dataReader;

        ///// <summary>
        ///// 数据库连接字符串定义
        ///// </summary>
        //private SQLiteConnectionStringBuilder dbConnectionstr;

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="connectionString">连接SQLite库字符串</param>
        /*
        public SqLiteHelper(string connectionString)
        {
            try
            {
                dbConnection = new SQLiteConnection();
                dbConnectionstr = new SQLiteConnectionStringBuilder();
                dbConnectionstr.DataSource = connectionString;
                dbConnectionstr.Version = 3;
                //dbConnectionstr.Password = "admin";      //设置密码，SQLite ADO.NET实现了数据库密码保护
                dbConnection.ConnectionString = dbConnectionstr.ToString();
                dbConnection.Open();
            }
            catch (Exception e)
            {
                Log(e.ToString());
                Log4Helper.Error(e.Message);
            }
        }
        */

        /// <summary>
        /// 创建数据库文件
        /// </summary>
        /// <param name="DBFilePath"></param>
        /// <returns></returns>
        public static Boolean NewDbFile(string DBFilePath)
        {
            try
            {
                SQLiteConnection.CreateFile(DBFilePath);
                return true;
            }
            catch (Exception ex)
            {
                Log4Helper.Error("新建数据库文件" + DBFilePath + "失败：" + ex.Message);
                throw new Exception("新建数据库文件" + DBFilePath + "失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="queryString">SQL命令字符串</param>
        public static SQLiteDataReader ExecuteQuery(string queryString)
        {
            Log4Helper.Debug("执行sql命令开始");
            Log4Helper.Debug("语句是：" + queryString);
            try
            {
                //dbCommand = dbConnection.CreateCommand();
                //dbCommand.CommandText = queryString;       //设置SQL语句
                //dataReader = dbCommand.ExecuteReader();
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=BilibiliPush.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
                    {
                        dataReader = cmd.ExecuteReader();
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Log(e.Message);
                Log4Helper.Error(e.Message);
            }
            Log4Helper.Debug("执行sql命令结束");
            return dataReader;
        }

        ///// <summary>
        ///// 关闭数据库连接
        ///// </summary>
        /*
        public static void CloseConnection()
        {
            //销毁Command
            if (dbCommand != null)
            {
                dbCommand.Cancel();
            }
            dbCommand = null;
            //销毁Reader
            if (dataReader != null)
            {
                dataReader.Close();
            }
            dataReader = null;
            //销毁Connection
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
            dbConnection = null;

        }
        */

        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="tableName">数据表名称</param>
        /// <returns>返回:是/否</returns>
        public static bool CheckTable(string tableName)
        {
            try
            {
                Log4Helper.Debug("调用CheckTable结束");
                string queryString = "SELECT count(*) FROM sqlite_master WHERE type= 'table' AND name = '" + tableName + "' ;";
                Log4Helper.Debug("sql语句：" + queryString);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=BilibiliPush.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
                    {
                        dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            if (dataReader[0].ToString() != "0")
                            {
                                return true;
                            }
                        }
                    }
                    dataReader.Close();
                    dataReader.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception e)
            {
                Log4Helper.Error(e.Message);
            }
            Log4Helper.Debug("调用CheckTable结束");
            return false;
        }

        /// <summary>
        /// 读取整张数据表
        /// </summary>
        /// <returns>The full table.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="l"></param>
        public static String[] ReadFullTable(string tableName, int l)
        {
            string[] arrData = new string[l];
            try
            {
                Log4Helper.Debug("调用ReadFullTable开始");
                string queryString = "SELECT * FROM " + tableName;  //获取所有可用的字段
                Log4Helper.Debug("sql语句：" + queryString);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=BilibiliPush.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
                    {
                        dataReader = cmd.ExecuteReader();
                        int i = 0;
                        while (dataReader.Read())
                        {
                            arrData[i] = dataReader.GetString(dataReader.GetOrdinal("UID"));
                            i++;
                        }
                    }
                    dataReader.Close();
                    dataReader.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception e)
            {
                Log4Helper.Error(e.Message);
            }
            Log4Helper.Debug("调用ReadFullTable结束");
            return arrData;
        }

        /// <summary>
        /// 读取整张数据表
        /// </summary>
        /// <returns>The full table.</returns>
        /// <param name="tableName">数据表名称</param>
        public static SQLiteDataReader ReadFullTable(string tableName)
        {
            try
            {
                Log4Helper.Debug("调用ReadFullTable开始");
                string queryString = "SELECT * FROM " + tableName;  //获取所有可用的字段
                Log4Helper.Debug("sql语句：" + queryString);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=BilibiliPush.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
                    {
                        dataReader = cmd.ExecuteReader();
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Log4Helper.Error(e.Message);
            }
            Log4Helper.Debug("调用ReadFullTable结束");
            return dataReader;
        }

        /// <summary>
        /// 计算整张表的行数
        /// </summary>
        /// <returns>count.</returns>
        /// <param name="tableName">数据表名称</param>
        public static int CountRows(string tableName)
        {
            int count = 0;
            try
            {
                Log4Helper.Debug("调用CountRows开始");
                string queryString = "SELECT * FROM " + tableName;  //获取所有可用的字段
                Log4Helper.Debug("sql语句：" + queryString);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=BilibiliPush.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
                    {
                        dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            count++;
                        }
                        Log4Helper.Debug("调用CountRows结束");
                    }
                    dataReader.Close();
                    dataReader.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception e)
            {
                Log4Helper.Error(e.Message);
            }
            return count;
        }

        /// <summary>
        /// 读取指定字段的值
        /// </summary>
        /// <returns>count.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="item"></param>
        /// <param name="colName"></param>
        /// <param name="colValue"></param>
        public static string ReadValue(string tableName, string item, string colName, string colValue)
        {
            string str = "";
            //查询数据库里当前colName对应的item
            try
            {
                Log4Helper.Debug("调用ReadValue开始");
                string queryString = "SELECT " + item + " FROM " + tableName + " where " + colName + " = " + colValue;
                Log4Helper.Debug("sql语句：" + queryString);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=BilibiliPush.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
                    {
                        dataReader = cmd.ExecuteReader();
                        while (dataReader.Read())
                        {
                            if ((dataReader[0].ToString() == "") || (dataReader[0] == null))
                            {
                                str = "";
                                break;
                            }
                            else
                            {
                                str = dataReader[0].ToString();
                                break;
                            }
                        }
                    }
                    dataReader.Close();
                    dataReader.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception e)
            {
                Log4Helper.Error(e.Message);
            }
            Log4Helper.Debug("调用ReadValue结束");
            return str;
        }


        /// <summary>
        /// 向指定数据表中插入数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="values">插入的数值</param>
        public static void InsertValues(string tableName, string[] values)
        {
            ////获取数据表中字段数目
            //int fieldCount = ReadFullTable(tableName).FieldCount;
            ////当插入的数据长度不等于字段数目时引发异常
            //if (values.Length != fieldCount)
            //{
            //    throw new SQLiteException("values.Length!=fieldCount");
            //}
            try
            {
                Log4Helper.Debug("调用InsertValues开始");
                string queryString = "INSERT INTO " + tableName + " VALUES (" + "'" + values[0] + "'";
                for (int i = 1; i < values.Length; i++)
                {
                    queryString += ", " + "'" + values[i] + "'";
                }
                queryString += " )";
                Log4Helper.Debug("sql语句：" + queryString);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=BilibiliPush.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
                    {
                        dataReader = cmd.ExecuteReader();
                    }
                    dataReader.Close();
                    dataReader.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception e)
            {
                Log4Helper.Error(e.Message);
            }
            Log4Helper.Debug("调用InsertValues结束");
        }

        /// <summary>
        /// 更新指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        /// <param name="key">关键字</param>
        /// <param name="value">关键字对应的值</param>
        /// <param name="operation">运算符：=,<,>,...，默认“=”</param>
        public static SQLiteDataReader UpdateValues(string tableName, string colNames, string colValues, string key, string operation, string value)
        {
            // operation="=";  //默认
            try
            {
                Log4Helper.Debug("调用UpdateValues开始");
                string queryString = "UPDATE " + tableName + " SET " + colNames + "=" + "'" + colValues + "'" + " WHERE " + key + operation + "'" + value + "'";
                Log4Helper.Debug("sql语句：" + queryString);
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=BilibiliPush.db"))
                {
                    conn.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(queryString, conn))
                    {
                        dataReader = cmd.ExecuteReader();
                    }
                    dataReader.Close();
                    dataReader.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception e)
            {
                Log4Helper.Error(e.Message);
            }
            Log4Helper.Debug("调用UpdateValues结束");
            return dataReader;
        }

        /// <summary>
        /// 更新指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        /// <param name="key">关键字</param>
        /// <param name="value">关键字对应的值</param>
        /// <param name="operation">运算符：=,<,>,...，默认“=”</param>
        public static SQLiteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key, string value, string operation)
        {
            // operation="=";  //默认
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length)
            {
                throw new SQLiteException("colNames.Length!=colValues.Length");
            }
            string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + "'" + colValues[0] + "'";

            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += ", " + colNames[i] + "=" + "'" + colValues[i] + "'";
            }
            queryString += " WHERE " + key + operation + "'" + value + "'";

            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 更新指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        /// <param name="key">关键字</param>
        /// <param name="value">关键字对应的值</param>
        /// <param name="operation">运算符：=,<,>,...，默认“=”</param>
        public SQLiteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key1, string value1, string operation, string key2, string value2)
        {
            // operation="=";  //默认
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length)
            {
                throw new SQLiteException("colNames.Length!=colValues.Length");
            }
            string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + "'" + colValues[0] + "'";

            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += ", " + colNames[i] + "=" + "'" + colValues[i] + "'";
            }
            //表中已经设置成int类型的不需要再次添加‘单引号’，而字符串类型的数据需要进行添加‘单引号’
            queryString += " WHERE " + key1 + operation + "'" + value1 + "'" + "OR " + key2 + operation + "'" + value2 + "'";

            return ExecuteQuery(queryString);
        }


        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        /// <param name="operations"></param>
        public SQLiteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] colValues, string[] operations)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SQLiteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + "'" + colValues[0] + "'";
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += "OR " + colNames[i] + operations[0] + "'" + colValues[i] + "'";
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        /// <param name="operations"></param>
        public SQLiteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] colValues, string[] operations)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SQLiteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + "'" + colValues[0] + "'";

            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += " AND " + colNames[i] + operations[i] + "'" + colValues[i] + "'";
            }
            return ExecuteQuery(queryString);
        }


        /// <summary>
        /// 创建数据表
        /// </summary> +
        /// <returns>The table.</returns>
        /// <param name="tableName">数据表名</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colTypes">字段名类型</param>
        public SQLiteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
        {
            string queryString = "CREATE TABLE IF NOT EXISTS " + tableName + "( " + colNames[0] + " " + colTypes[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += ", " + colNames[i] + " " + colTypes[i];
            }
            queryString += "  ) ";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// Reads the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <param name="tableName">Table name.</param>
        /// <param name="items">Items.</param>
        /// <param name="colNames">Col names.</param>
        /// <param name="operations">Operations.</param>
        /// <param name="colValues">Col values.</param>
        public SQLiteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
        {
            string queryString = "SELECT " + items[0];
            for (int i = 1; i < items.Length; i++)
            {
                queryString += ", " + items[i];
            }
            queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[i] + " ";
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 本类log
        /// </summary>
        /// <param name="s"></param>
        static void Log(string s)
        {
            Console.WriteLine("class SqLiteHelper:::" + s);
        }
    }
}

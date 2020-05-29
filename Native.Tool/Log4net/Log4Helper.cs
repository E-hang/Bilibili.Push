using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Native.Tool.Log4net
{
    public class Log4Helper
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");  // 这里的 loginfo 和 log4net.config 里的名字要一样
        //public static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");// 这里的 logerror 和 log4net.config 里的名字要一样

        public static void Debug(object message)
        {
            if (loginfo.IsDebugEnabled)
            {
                loginfo.Debug(message);
            }
        }

        public static void Debug(object message, Exception ex)
        {
            if (loginfo.IsDebugEnabled)
            {
                loginfo.Debug(message, ex);
            }
        }

        public static void Info(object message)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(message);
            }
        }

        public static void Info(object message, Exception ex)
        {
            if (loginfo.IsInfoEnabled)
            {
                loginfo.Info(message, ex);
            }
        }

        public static void Warn(object message)
        {
            if (loginfo.IsWarnEnabled)
            {
                loginfo.Warn(message);
            }
        }

        public static void Warn(object message, Exception ex)
        {
            if (loginfo.IsWarnEnabled)
            {
                loginfo.Warn(message, ex);
            }
        }

        public static void Error(object message)
        {
            if (loginfo.IsErrorEnabled)
            {
                loginfo.Error(message);
            }
        }

        public static void Error(object message, Exception ex)
        {
            if (loginfo.IsErrorEnabled)
            {
                loginfo.Error(message, ex);
            }
        }

        public static void Fatal(object message)
        {
            if (loginfo.IsFatalEnabled)
            {
                loginfo.Fatal(message);
            }
        }

        public static void Fatal(object message, Exception ex)
        {
            if (loginfo.IsFatalEnabled)
            {
                loginfo.Fatal(message, ex);
            }
        }
    }
}
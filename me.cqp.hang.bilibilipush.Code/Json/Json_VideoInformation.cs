using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.hang.bilibilipush.Code.Json
{
    public class Json_VideoInformation
    {
        public class Vlist
        {
            public string comment { get; set; }
            public string typeid { get; set; }
            public string play { get; set; }
            public string pic { get; set; }
            public string subtitle { get; set; }
            public string description { get; set; }
            public string copyright { get; set; }
            public string title { get; set; }
            public string review { get; set; }
            public string author { get; set; }
            public string mid { get; set; }
            public string is_union_video { get; set; }
            public string created { get; set; }
            public string length { get; set; }
            public string video_review { get; set; }
            public string is_pay { get; set; }
            public string favorites { get; set; }
            public string aid { get; set; }
            public string hide_click { get; set; }
        }

        public class Data
        {
            public List<Vlist> vlist { get; set; }
            public string count { get; set; }
            public string pages { get; set; }
        }

        public class RootObject
        {
            public string status { get; set; }
            public Data data { get; set; }
        }

    }
}

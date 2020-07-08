using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace me.cqp.hang.bilibilipush.Code.Json
{
    public class Json_UpInformation
    {
        public class Official
        {
            public string role { get; set; }
            public string title { get; set; }
            public string desc { get; set; }
            public string type { get; set; }
        }

        public class Label
        {
            public string path { get; set; }
            public string text { get; set; }
            public string label_theme { get; set; }
        }

        public class Vip
        {
            public string type { get; set; }
            public string status { get; set; }
            public string theme_type { get; set; }
            public Label label { get; set; }
            public string avatar_subscript { get; set; }
            public string nickname_color { get; set; }
        }

        public class Pendant
        {
            public string pid { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public string expire { get; set; }
            public string image_enhance { get; set; }
        }

        public class Nameplate
        {
            public string nid { get; set; }
            public string name { get; set; }
            public string image { get; set; }
            public string image_small { get; set; }
            public string level { get; set; }
            public string condition { get; set; }
        }

        public class Theme
        {
        }

        public class Sys_notice
        {
        }

        public class Data
        {
            public string mid { get; set; }
            public string name { get; set; }
            public string sex { get; set; }
            public string face { get; set; }
            public string sign { get; set; }
            public string rank { get; set; }
            public string level { get; set; }
            public string jointime { get; set; }
            public string moral { get; set; }
            public string silence { get; set; }
            public string birthday { get; set; }
            public string coins { get; set; }
            public string fans_badge { get; set; }
            public Official official { get; set; }
            public Vip vip { get; set; }
            public Pendant pendant { get; set; }
            public Nameplate nameplate { get; set; }
            public string is_followed { get; set; }
            public string top_photo { get; set; }
            public Theme theme { get; set; }
            public Sys_notice sys_notice { get; set; }
        }

        public class RootObject
        {
            public string code { get; set; }
            public string message { get; set; }
            public string ttl { get; set; }
            public Data data { get; set; }
        }
    }
}

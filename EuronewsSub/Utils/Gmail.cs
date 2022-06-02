using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EuronewsSub.Utils
{
    public class Gmail
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
        public List<string> Attachments { get; set; }
        public string MsgID { get; set; }
        public List<string> Hrefs { get; set; }
    }
}

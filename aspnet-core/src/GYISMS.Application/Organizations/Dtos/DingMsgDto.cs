using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Organizations.Dtos
{
    public class DingMsgDto
    {
        public DingMsgDto()
        {
            msg = new DingMsg();
        }
        public int agent_id { get; set; }

        public string userid_list { get; set; }

        public string dept_id_list { get; set; }

        public bool to_all_user { get; set; }

        public DingMsg msg { get; set; }
    }

    public class DingMsg
    {
        public DingMsg()
        {
            link = new DingLinkMsg();
        }
        public string msgtype { get; set; }

        public DingLinkMsg link { get; set; }
    }

    public class DingLinkMsg
    {
        public string messageUrl { get; set; }

        public string picUrl { get; set; }

        public string text { get; set; }

        public string title { get; set; }
    }
}

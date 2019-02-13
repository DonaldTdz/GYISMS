using GYISMS.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Organizations.Dtos
{
    public class DingUserListDto : DingBase
    {
        public bool hasMore { get; set; }
        public List<UserlistDomain> userlist { get; set; }
    }

    public class UserlistDomain
    {
        public string unionid { get; set; }

        public string tel { get; set; }

        public string remark { get; set; }

        public string position { get; set; }

        public string orgEmail { get; set; }

        public long order { get; set; }

        public string name { get; set; }

        public string mobile { get; set; }

        public string jobnumber { get; set; }

        public string userid { get; set; }

        public bool isLeader { get; set; }

        public bool isBoss { get; set; }

        public bool isAdmin { get; set; }

        public string hiredDate { get; set; }

        public string extattr { get; set; }

        public string email { get; set; }

        public string dingId { get; set; }

        public string department { get; set; }

        public string avatar { get; set; }

        public bool active { get; set; }

        public bool isHide { get; set; }

        public string workPlace { get; set; }
    }
}

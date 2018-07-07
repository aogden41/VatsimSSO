using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VatsimSSO
{
    public class SSOReturnData
    {
        public SSORequestStatus Request { get; set; }
        public SSOUser User { get; set; }
        public string Raw { get; set; } = "";
    }

    public class SSOUser
    {
        public string ID { get; set; }
        public string Name_First { get; set; }
        public string Name_Last { get; set; }
        public SSOATCRating Rating { get; set; }
        public SSOPilotRating Pilot_Rating { get; set; }
        public string Experience { get; set; }
        public string Reg_Date { get; set; }
        public SSOUserArea Country { get; set; }
        public SSOUserArea Region { get; set; }
        public SSOUserArea Division { get; set; }
        public SSOUserArea Subdivision { get; set; }
    }

    public class SSOATCRating
    {
        public string ID { get; set; }
        public string Short { get; set; }
        public string Long { get; set; }
        public string GRP { get; set; }
    }

    public class SSOPilotRating
    {
        public string Rating { get; set; }
    }

    public class SSOUserArea
    {
        public string Code { get; set; }
        public string Name { get; set; }
    }
}

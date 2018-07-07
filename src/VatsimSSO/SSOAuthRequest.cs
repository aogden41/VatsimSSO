using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VatsimSSO
{
    public class SSOAuthRequest
    {
        public SSORequestStatus Request { get; set; }
        public SSORequestToken Token { get; set; }
        public string Raw { get; set; } = "";
    }

    public class SSORequestStatus
    {
        public string Result { get; set; }
        public string Message { get; set; }
    }

    public class SSORequestToken
    {
        public string oauth_token { get; set; }
        public string oauth_token_secret { get; set; }
        public string oauth_callback_confirmed { get; set; }
    }
}

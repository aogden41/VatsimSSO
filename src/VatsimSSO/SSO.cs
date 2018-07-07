using System;
using System.IO;
using System.Net;
using OAuth;
using Newtonsoft.Json;

namespace VatsimSSO
{
    public sealed class VatsimSSO
    {   
        /*
        / Properties
        */
        private string ConsumerKey { get; set; } // REQUIRED

        private string ConsumerSecret { get; set; } // REQUIRED

        private string BaseUrl { get; set; } // Defaults to https://cert.vatsim.net/sso/api/

        private string SignatureMethod { get; set; } // Currently only supports HMAC

        private string CallbackUrl { get; set; } // REQUIRED

        private string Verifier { get; set; } // Only required when requesting protected resources (User data)

        private string Token { get; set; } // Only required when requesting protected resources (User data)

        private string TokenSecret { get; set; } // Only required when requesting protected resources (User data)

        private string JsonRequestData { get; set; } // Raw json string for the request token

        private string JsonReturnData { get; set; } // Raw json string for the return user data

        // Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ConsumerKey">OAuth Consumer Key - REQUIRED</param>
        /// <param name="ConsumerSecret">OAuth Consumer Secret - REQUIRED</param>
        /// <param name="BaseUrl">OAuth Base URL (defaults to cert)</param>
        /// <param name="CallbackUrl">Link to redirect back to after login</param>
        /// <param name="Verifier">OAuth Verifier</param>
        /// <param name="Token">OAuth Access Token</param>
        /// <param name="SSOTokenSecret">OAuth Token Secret</param>
        public VatsimSSO(string ConsumerKey, string ConsumerSecret, string BaseUrl = "https://cert.vatsim.net/sso/api/", string CallbackUrl = null, string Verifier = null, string Token = null, string TokenSecret = null)
        {
            this.ConsumerKey = ConsumerKey;
            this.ConsumerSecret = ConsumerSecret;
            this.BaseUrl = BaseUrl;
            this.CallbackUrl = CallbackUrl;
            this.SignatureMethod = "HMAC";
            this.Verifier = Verifier;
            this.Token = Token;
            this.TokenSecret = TokenSecret;
        }

        /*
        METHODS
        */

        #region Login token
        /// <summary>
        /// Gets a login token object to use in client authentication
        /// </summary>
        /// <returns>Returns a custom object with token and result information</returns>
        public SSOAuthRequest GetRequestToken()
        {
            // Check if callback url has been provided
            if (CallbackUrl == null) throw new Exception("Please provide a valid callback url.");

            // Base request object
            OAuthRequest client;

            // Instantiate the client object
            client = new OAuthRequest()
            {
                ConsumerKey = this.ConsumerKey,
                ConsumerSecret = this.ConsumerSecret,
                Method = "GET",
                CallbackUrl = CallbackUrl,
                Type = OAuthRequestType.RequestToken,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                RequestUrl = this.BaseUrl + "login_token/",
            };

            // Generate auth header
            string Auth = client.GetAuthorizationHeader();

            // Create the web request and add auth header
            var Request = (HttpWebRequest)WebRequest.Create(client.RequestUrl);
            Request.Headers.Add("Authorization", Auth);

            // Get the response and read to JSON
            var Response = (HttpWebResponse)Request.GetResponse();
            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string Json = Reader.ReadToEnd();

            // Deserialize the JSON string into a dynamic object
            var Token = JsonConvert.DeserializeObject<SSOAuthRequest>(Json);

            if (Token != null)
                Token.Raw = Json;

            // Return the token object from the JSON string (has three values: oauth_token, oauth_token_secret, oauth_callback_confirmed)
            return Token;
        }

        #endregion
        /// <summary>
        /// Returns the user data in JSON using the token, token secret and verifier provided.
        /// </summary>
        /// <returns>JSON string</returns>
        public SSOReturnData ReturnData()
        {
            // Check if verifier has been provided
            if (Verifier == null) throw new Exception("Please provide a valid verifier");
            
            // Check if token has been provided
            if (Token == null) throw new Exception("Please provide a valid token");

            // Check if token secret has been provided
            if (TokenSecret == null) throw new Exception("Please provide a valid token secret");
            // Base request object
            OAuthRequest client;

            // Instantiate the client object
            client = new OAuthRequest()
            {
                ConsumerKey = this.ConsumerKey,
                ConsumerSecret = this.ConsumerSecret,
                Method = "GET",
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                Token = this.Token,
                TokenSecret = TokenSecret,
                Verifier = this.Verifier,
                RequestUrl = this.BaseUrl + "login_return/"
            };

            // Generate auth header
            string Auth = client.GetAuthorizationHeader();

            // Create webrequest and add the auth header
            var Request = (HttpWebRequest)WebRequest.Create(client.RequestUrl);
            Request.Headers.Add("Authorization", Auth);

            // Get response and read result to JSON
            var Response = (HttpWebResponse)Request.GetResponse();
            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string Json = Reader.ReadToEnd();

            // Deserialize the JSON string into a dynamic object
            var Data = JsonConvert.DeserializeObject<SSOReturnData>(Json);

            if (Data != null)
                Data.Raw = Json;

            // Return the user
            return Data;
        }
    }
}

using System;
using System.IO;
using System.Net;
using OAuth;
using Newtonsoft.Json;
using System.Web;

namespace VatsimSSO
{
    public sealed class VatsimSSO
    {   
		/// <summary>
		///		The consumer key.
		/// </summary>
        private string ConsumerKey { get; set; }

		/// <summary>
		///		The consumer secret.
		/// </summary>
        private string ConsumerSecret { get; set; }

		/// <summary>
		///		The base VATSIM SSO url.
		///		https://cert.vatsim.net/sso/api/ by default.
		/// </summary>
		private string BaseUrl { get; set; }

		/// <summary>
		///		The signature method.
		/// </summary>
		private readonly string SignatureMethod = "HMAC";

		/// <summary>
		///		The callback url.
		/// </summary>
        private string CallbackUrl { get; set; }

		/// <summary>
		///		The OAuth verifier.
		///		Only required when requesting protected resources (User data).
		/// </summary>
		private string Verifier { get; set; }

		/// <summary>
		///		The OAuth token.
		///		Only required when requesting protected resources (User data).
		/// </summary>
		private string Token { get; set; }

		/// <summary>
		///		The OAuth token sected.
		///		Only required when requesting protected resources (User data).
		/// </summary>
		private string TokenSecret { get; set; }

		/// <summary>
		///		The raw json string for the request token.
		/// </summary>
		private string JsonRequestData { get; set; }

		/// <summary>
		///		The raw json string for the return user data
		/// </summary>
		private string JsonReturnData { get; set; } 

        /// <summary>
        ///		The constructor
        /// </summary>
        /// <param name="ConsumerKey">
		///		The OAuth Consumer Key.
		///	</param>
        /// <param name="ConsumerSecret">
		///		The OAuth Consumer Secret.
		///	</param>
        /// <param name="BaseUrl">
		///		OAuth Base URL
		///	</param>
        /// <param name="CallbackUrl">
		///		Link to redirect back to after login
		///	</param>
        /// <param name="Verifier">
		///		OAuth Verifier
		///	</param>
        /// <param name="Token">
		///		OAuth Access Token
		///	</param>
        /// <param name="SSOTokenSecret">
		///		OAuth Token Secret
		///	</param>
        public VatsimSSO(string ConsumerKey, string ConsumerSecret, string BaseUrl = "https://cert.vatsim.net/sso/api/", string CallbackUrl = null, string Verifier = null, string Token = null, string TokenSecret = null)
        {
            this.ConsumerKey = ConsumerKey;
            this.ConsumerSecret = ConsumerSecret;
            this.BaseUrl = BaseUrl;
            this.CallbackUrl = CallbackUrl;
            // this.SignatureMethod = "HMAC";
            this.Verifier = Verifier;
            this.Token = Token;
            this.TokenSecret = TokenSecret;
        }

        /// <summary>
        ///		Get's the SSO token information.
        /// </summary>
        /// <returns>
		///		An SSO authentication response. See <see cref="SSOAuthResponse"/>.
		///	</returns>
        public SSOAuthResponse GetRequestToken()
        {
            // Check if callback url has not been provided
            if (string.IsNullOrEmpty(CallbackUrl.Trim())) throw new NullReferenceException("Callback Url not provided. Please do so in the constructor.");

            // Base request object
            OAuthRequest client = new OAuthRequest()
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
            string auth = client.GetAuthorizationQuery();

            // Create the web request and add auth header
            var url = client.RequestUrl + '?' + auth;
            var request = (HttpWebRequest)WebRequest.Create(url);

            // Get the response and read to JSON
            var response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();

            // Deserialize the JSON string into a dynamic object
            var token = JsonConvert.DeserializeObject<SSOAuthResponse>(json);

            if (token != null)
                token.Raw = json;

            // Return the token object
            return token;
        }

        /// <summary>
        ///		Get's the user data.
        /// </summary>
        /// <returns>
		///		The user data. See <see cref="SSOReturnData"/>.
		///	</returns>
        public SSOReturnData ReturnData()
        {
			// Check if verifier has been provided
			if (string.IsNullOrEmpty(Verifier.Trim())) throw new NullReferenceException("The OAuth Verifier was null or empty.");
            
            // Check if token has been provided
            if (string.IsNullOrEmpty(Token.Trim())) throw new NullReferenceException("The OAuth Token was null or empty.");

			// Check if token secret has been provided
			if (string.IsNullOrEmpty(TokenSecret.Trim())) throw new NullReferenceException("The OAuth Token Secret was null or empty.");
			
			// Base request object
			OAuthRequest client = new OAuthRequest()
            {
                ConsumerKey = this.ConsumerKey,
                ConsumerSecret = this.ConsumerSecret,
                Method = "GET",
                Type = OAuthRequestType.ProtectedResource,
                SignatureMethod = OAuthSignatureMethod.HmacSha1,
                Token = this.Token,
                TokenSecret = TokenSecret,
                Verifier = this.Verifier,
                RequestUrl = this.BaseUrl + "login_return/"
            };

            // Generate auth header
            string auth = client.GetAuthorizationQuery();

            // Create webrequest and add the auth header
            var url = new Uri(client.RequestUrl + '?' + auth);
            var request = (HttpWebRequest)WebRequest.Create(url.AbsoluteUri);

            // Get response and read result to JSON
            var response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();

            // Deserialize the JSON string into a dynamic object
            var data = JsonConvert.DeserializeObject<SSOReturnData>(json);

            if (data != null)
                data.Raw = json;

            // Return the user
            return data;
        }
    }
}

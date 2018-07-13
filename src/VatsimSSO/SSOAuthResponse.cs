using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VatsimSSO
{
	/// <summary>
	///		The SSO Authentication response.
	/// </summary>
    public class SSOAuthResponse
    {
		/// <summary>
		///		The status information.
		///		See <see cref="SSOResponseStatus"/>.
		/// </summary>
        public SSOResponseStatus Request { get; set; }

		/// <summary>
		///		The token information.
		///		See <see cref="SSOResponseToken"/>.
		/// </summary>
        public SSOResponseToken Token { get; set; }

		/// <summary>
		///		The raw JSON received from VATSIM.
		/// </summary>
        public string Raw { get; set; } = "";
    }

	/// <summary>
	///		The Status data.
	/// </summary>
    public class SSOResponseStatus
	{
		/// <summary>
		///		The result.
		/// </summary>
        public string Result { get; set; }

		/// <summary>
		///		The message.
		/// </summary>
        public string Message { get; set; }
    }

	/// <summary>
	///		The token data.
	/// </summary>
	/// Todo: Make use of [JsonProperty] to tidy up the class naming scheme.
	/// Todo: eg: [JsonProperty("oauth_token")]
	public class SSOResponseToken
	{
		/// <summary>
		///		The token.
		/// </summary>
        public string oauth_token { get; set; }

		/// <summary>
		///		The token secret.
		/// </summary>
        public string oauth_token_secret { get; set; }

		/// <summary>
		///		Whether or not the callback was confirmed.
		/// </summary>
        public string oauth_callback_confirmed { get; set; }
    }
}

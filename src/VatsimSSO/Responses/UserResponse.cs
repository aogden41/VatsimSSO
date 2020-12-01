namespace VatsimSingleSignOn.Responses
{
	using Newtonsoft.Json;
	using VatsimSingleSignOn.VATSIMData;

	/// <summary>
	/// 	The data received from VATSIM SSO.
	/// </summary>
	public class UserResponse : IReponse
	{
		/// <summary>
		/// 	Gets or sets the <see cref="ResponseStatus"/>.
		/// </summary>
		[JsonProperty("Response")]
		public ResponseStatus Status { get; set; }

		/// <summary>
		///		Gets ot sets the raw JSON string received from VATSIM SSO.
		/// </summary>
		public string Raw { get; set; } = "";

		/// <summary>
		/// 	Gets or sets the <see cref="global::VatsimSSO.User"/>.
		/// </summary>
		public User User { get; set; }
	}
}
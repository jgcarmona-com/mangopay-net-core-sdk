namespace MangoPay.SDK.Core
{
    /// <summary>
    /// MangoPay Configuration Object
    /// </summary>
    public class MangoPayApiConfiguration
    {
        /// <summary>
        /// Gets or sets the Client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; set; }
        /// <summary>
        /// Gets or sets the client password.
        /// </summary>
        /// <value>
        /// The client password.
        /// </value>
        public string ClientPassword { get; set; }
        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BaseUrl { get; set; }
        /// <summary>
        /// Gets or sets the timeout.
        /// </summary>
        /// <value>
        /// The timeout in milliseconds.
        /// </value>
        /// <remarks>Set Zero to use the default .Net Timeout </remarks>
        public int Timeout { get; set; }
        /// <summary>
        /// Gets or sets the API version.
        /// </summary>
        /// <value>
        /// The API version.
        /// </value>
        public string ApiVersion { get; set; }
    }
}

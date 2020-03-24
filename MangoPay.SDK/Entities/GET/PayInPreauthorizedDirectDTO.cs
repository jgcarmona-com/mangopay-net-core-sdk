using System;

namespace MangoPay.SDK.Entities.GET
{
    public class PayInPreauthorizedDirectDTO : PayInDTO
    {
        /// <summary>Pre-authorization identifier.</summary>
        public string PreauthorizationId { get; set; }

        /// <summary>Card identifier.</summary>
        public string CardId { get; set; }

        /// <summary>SecureMode { DEFAULT, FORCE }.</summary>
        public string SecureMode { get; set; }

        /// <summary>Secure mode return URL.</summary>
        public string SecureModeReturnURL { get; set; }
    }
}

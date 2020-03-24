using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Converters;
using MangoPay.SDK.Core.Enumerations;
using Newtonsoft.Json;
using System;

namespace MangoPay.SDK.Entities.GET
{
    /// <summary>UserNatural entity.</summary>
    public sealed class UserNaturalDTO : UserDTO
    {
        /// <summary>First name.</summary>
        public string FirstName { get; set; }

        /// <summary>Last name.</summary>
        public string LastName { get; set; }

        /// <summary>Address.</summary>
		public Address Address { get; set; }

        /// <summary>Date of birth (UNIX timestamp).</summary>
        [JsonConverter(typeof(Core.Converters.UnixDateTimeConverter))]
        public DateTime? Birthday { get; set; }

        /// <summary>Place of birth.</summary>
        public string Birthplace { get; set; }

        /// <summary>User's country.</summary>
		[JsonConverter(typeof(EnumerationConverter))]
        public CountryIso Nationality { get; set; }

        /// <summary>Country of residence.</summary>
		[JsonConverter(typeof(EnumerationConverter))]
        public CountryIso CountryOfResidence { get; set; }

        /// <summary>User's occupation.</summary>
        public string Occupation { get; set; }

        /// <summary>Income range. One of UserNatural.IncomeRanges constants or null, if not specified.</summary>
        public int? IncomeRange { get; set; }

        /// <summary>Proof of identity.</summary>
        public string ProofOfIdentity { get; set; }

        /// <summary>Proof of address.</summary>
        public string ProofOfAddress { get; set; }
    }
}

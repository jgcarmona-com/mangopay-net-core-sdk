using MangoPay.SDK.Core.Enumerations;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace MangoPay.SDK.Entities.GET
{
    public class SingleSignOnDTO : EntityBase
	{
		/// <summary>The name of the user.</summary>
		public string FirstName { get; set; }

		/// <summary>The last name of the user.</summary>
		public string LastName { get; set; }

		/// <summary>Email address.</summary>
		public string Email { get; set; }

		/// <summary>Wheter the SSO is active or not.</summary>
		public bool Active { get; set; }

		/// <summary>Wheter the SSO is active or not.</summary>
		public InvitationStatus InvitationStatus { get; set; }

		/// <summary>Date of the latest authentification.</summary>
		[JsonConverter(typeof(UnixDateTimeConverter))]
		public DateTime? LastLoginDate { get; set; }

		/// <summary>Permission group ID assigned to this SSO.</summary>
		public string PermissionGroupId { get; set; }

		/// <summary>An ID for the client.</summary>
		public string ClientId { get; set; }
	}
}

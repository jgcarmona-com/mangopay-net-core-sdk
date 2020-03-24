﻿namespace MangoPay.SDK.Entities
{
    /// <summary>
    /// Shipping Address
    /// </summary>
    public class ShippingAddress
	{
		public ShippingAddress(string recipientName, Address address)
		{
			RecipientName = recipientName;
			Address = address;
		}
		
		/// <summary>Recipient name for PayPal shipping address.</summary>
		public string RecipientName { get; set; }

		/// <summary>The address.</summary>
		public Address Address { get; set; }
	}
}

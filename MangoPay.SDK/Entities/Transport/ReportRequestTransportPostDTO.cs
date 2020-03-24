using MangoPay.SDK.Core;
using MangoPay.SDK.Core.Enumerations;
using MangoPay.SDK.Entities.POST;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MangoPay.SDK.Entities.Transport
{
	internal class ReportRequestTransportPostDTO : EntityPostBase
	{
		/// <summary>Download file format.</summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public DownloadReportFormat DownloadFormat { get; set; }

		/// <summary>Callback URL.</summary>
		public string CallbackURL { get; set; }

		/// <summary>Type of the report.</summary>
		[JsonConverter(typeof(StringEnumConverter))]
		public ReportType? ReportType { get; set; }

		/// <summary>Sorting (defaults to: CreationDate, ascending).</summary>
		public string Sort { get; set; }

		/// <summary>If true, the report will be limited to the first 10 lines.</summary>
		public bool Preview { get; set; }

		/// <summary>Filters for the report list.</summary>
		public FilterReportsTransport Filters { get; set; }

		/// <summary>Allowed values: "Alias", "BankAccountId", "BankWireRef", "CardId", "CardType", "Country", "Culture", "Currency", "DeclaredDebitedFundsAmount", "DeclaredDebitedFundsCurrency", "DeclaredFeesAmount", "DeclaredFeesCurrency", "ExecutionType", "ExpirationDate", "PaymentType", "PreauthorizationId", "WireReference".</summary>
		public string[] Columns { get; set; }


		public ReportRequestPostDTO GetBusinessObject()
		{
			ReportRequestPostDTO result = new ReportRequestPostDTO(ReportType ?? Core.Enumerations.ReportType.TRANSACTIONS);

			result.CallbackURL = CallbackURL;
			result.Columns = Columns != null ? Columns.ToList<string>() : null;
			result.DownloadFormat = DownloadFormat;
			result.Preview = Preview;
			result.ReportType = ReportType;
			result.Sort = Sort;
			result.Tag = Tag;

			if (Filters != null) result.Filters = Filters.GetBusinessObject();

			return result;
		}

		public static ReportRequestTransportPostDTO CreateFromBusinessObject(ReportRequestPostDTO reportRequest)
		{
			ReportRequestTransportPostDTO result = new ReportRequestTransportPostDTO();

			result.CallbackURL = reportRequest.CallbackURL;
			result.Columns = reportRequest.Columns != null ? reportRequest.Columns.ToArray<string>() : null;
			result.DownloadFormat = reportRequest.DownloadFormat;
			result.Preview = reportRequest.Preview;
			result.ReportType = reportRequest.ReportType;
			result.Tag = reportRequest.Tag;

			if (reportRequest.Filters != null) result.Filters = FilterReportsTransport.CreateFromBusinessObject(reportRequest.Filters);

			if (string.IsNullOrWhiteSpace(reportRequest.Sort)) result.Sort = "CreationDate:ASC";
			else result.Sort = reportRequest.Sort;

			return result;
		}
	}
}

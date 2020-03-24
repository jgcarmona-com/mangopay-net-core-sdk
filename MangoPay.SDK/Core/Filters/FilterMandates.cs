using MangoPay.SDK.Core.Converters;
using MangoPay.SDK.Core.Enumerations;
using System;
using System.Collections.Generic;

namespace MangoPay.SDK.Core.Filters
{
    /// <summary>Filter for mandates list.</summary>
    public class FilterMandates
    {
        /// <summary>Mandate status.</summary>
        public MandateStatus Status { get; set; }

        /// <summary>End date: return only mandates that have CreationDate BEFORE this date.</summary>
        public DateTime? BeforeDate;

        /// <summary>Start date: return only mandates that have CreationDate AFTER this date.</summary>
        public DateTime? AfterDate;

        /// <summary>Gets map of fields and values.</summary>
        /// <returns>Returns collection of field_name-field_value pairs.</returns>
        public Dictionary<string, string> GetValues()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            UnixDateTimeConverter dateConverter = new UnixDateTimeConverter();

            if (Status != MandateStatus.NotSpecified) result.Add(Constants.STATUS, Status.ToString());
            if (BeforeDate.HasValue) result.Add(Constants.BEFOREDATE, dateConverter.ConvertToUnixFormat(BeforeDate).Value.ToString());
            if (AfterDate.HasValue) result.Add(Constants.AFTERDATE, dateConverter.ConvertToUnixFormat(AfterDate).Value.ToString());

            return result;
        }
    }

}

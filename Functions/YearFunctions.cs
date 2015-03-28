using System;

namespace MerchandiseCalendar
{
    public static partial class DateFunctions
    {
        /// <summary>
        /// Returns an object with data representing particulars of the merchandise year: Year, start and end dates and whether or not it has a 53rd week.
        /// </summary>
        /// <param name="year">
        /// The year you want to get the information for.
        /// </param>
        /// <returns>
        /// MerchYear
        /// </returns>
        public static MerchYear GetMerchYearInfo(int year)
        {
            return new MerchYear(year);
        }

        /// <summary>
        /// Retrieves the merchandise calendar year from the date.
        /// </summary>
        /// <param name="date">
        /// Date you wish to get the merchandise year from.
        /// </param>
        /// <returns>
        /// integer
        /// </returns>
        public static int GetYear(DateTime date)
        {
            var year = date.Year;
            var merchYear = GetMerchYearInfo(year);

            // if the date is not in the date range for the merchandise year it is part of the previous year.
            if (date >= merchYear.DateRange.StartDate && date <= merchYear.DateRange.EndDate)
                return year;

            return year - 1;
        }

        /// <summary>
        /// Returns date range from the beginning of the merchandise year to the date.
        /// </summary>
        /// <param name="date">
        /// The date representing the end of the "to date" time span.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// DateRange
        /// </returns>
        public static DateRange GetYearToDate(DateTime date,
            bool restated = false)
        {
            var year = GetYear(date);
            var startDate = GetPeriodDateRange(1, year, restated).StartDate;
            
            return new DateRange
            {
                StartDate = startDate,
                EndDate = date.ToEndOfDay()
            };
        } 
    }
}

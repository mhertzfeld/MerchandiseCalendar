using System;
using System.Linq;

namespace MerchandiseCalendar
{
    public static partial class DateFunctions
    {
        /// <summary>
        /// Retrieves the merchandise calendar week from the date. Returns 0 if restated is true and date falls within the "lost" week in a 53 week year restated for comparability.
        /// </summary>
        /// <param name="date">
        /// Date you wish to get the merchandise week from.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// integer
        /// </returns>
        public static int GetWeek(DateTime date,
            bool restated = false)
        {
            // Get information about the merchandise year.
            var merchYear = new MerchYear(GetYear(date));
            // TimeSpan used to calculate the number of weeks from the beginning of the year to the date.
            var timeSpan = date - merchYear.DateRange.StartDate;
            // Gets the number of days + 1 divided by 7, rounded up to the nearest whole number.
            var returnValue = (int)(Math.Ceiling((decimal)(timeSpan.Days + 1) / 7));

            // If the merch year has an extra week and the time period is restated for comparison, subtract one.
            if (merchYear.ExtraWeek && restated)
                returnValue -= 1;

            return returnValue;
        }

        /// <summary>
        /// Retrieves a date range for the merchandise week based on the week number and the year.
        /// </summary>
        /// <param name="week">
        /// The merchandise week you wish to get date range information for.
        /// </param>
        /// <param name="year">
        /// The merchandise year you wish to get the date range information for.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// DateRange
        /// </returns>
        public static DateRange GetWeekDateRange(int week,
            int year,
            bool restated = false)
        {
            // Make sure it's a valid week.
            ValidateWeek(week);

            // Get information about the merchandise year.
            var merchYear = GetMerchYearInfo(year);

            // If the year has an extra week and the time period is being restated for comparison, add one.
            if (merchYear.ExtraWeek && restated)
                week += 1;

            // Calculate values for the start and end dates.
            var startDate = merchYear.DateRange.StartDate.AddDays(7 * (week - 1));
            var endDate = merchYear.DateRange.StartDate.AddDays((7 * week) - 1).ToEndOfDay();

            return new DateRange
            {
                StartDate = startDate,
                EndDate = endDate
            };
        }

        /// <summary>
        /// Retrieves a date range for the merchandise week based on the date.
        /// </summary>
        /// <param name="date">
        /// The date you wish to get date range information for.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// DateRange
        /// </returns>
        public static DateRange GetWeekDateRange(DateTime date,
            bool restated = false)
        {
            return GetWeekDateRange(GetWeek(date, restated), GetYear(date), restated);
        }

        /// <summary>
        /// Retrieves date range from the beginning of the merchandise week up to the date.
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
        public static DateRange GetWeekToDate(DateTime date,
            bool restated = false)
        {
            var week = GetWeek(date, restated);
            var year = GetYear(date);
            // Only need to calculate the start date of the week range.
            var startDate = GetWeekDateRange(week, year, restated).StartDate;

            return new DateRange
            {
                StartDate = startDate,
                EndDate = date.ToEndOfDay()
            };
        }

        /// <summary>
        /// Retrieves date range from the beginning of the merchandise week up to the day of the week of the merchandise week.
        /// </summary>
        /// <param name="week">
        /// The merchandise week you wish to get the information for.
        /// </param>
        /// <param name="year">
        /// The year you wish to get the information for.
        /// </param>
        /// <param name="dayOfWeek">
        /// The day of the week you wish to get the information for.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// DateRange
        /// </returns>
        public static DateRange GetWeekToDate(int week,
            int year,
            DayOfWeek dayOfWeek,
            bool restated = false)
        {
            // Get a list of all dates between the date range for the week based on week number and year.
            var weekRange = GetAllDatesBetween(GetWeekDateRange(week, year, restated));
            // Get the first date in the list that matches the day of the week.
            var date = weekRange.First(x => x.DayOfWeek == dayOfWeek);

            return GetWeekToDate(date, restated);
        } 
    }
}

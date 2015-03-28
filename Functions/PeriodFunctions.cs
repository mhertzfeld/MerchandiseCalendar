using System;

namespace MerchandiseCalendar
{
    public static partial class DateFunctions
    {
        /// <summary>
        /// Returns the number of Sunday through Saturday weeks in a period.
        /// </summary>
        /// <param name="period">
        /// The period you wish to get the number of weeks for.
        /// </param>
        /// <returns>
        /// integer
        /// </returns>
        public static int WeeksInPeriod(int period)
        {
            /* Merchandise calendar quarters are based on a 4-5-4 week pattern, repeated 4 times to make up the year.
             * 5 week periods are 2, 5, 8 and 11, all others have 4 weeks. */
            if (period == 2 ||
                period == 5 ||
                period == 8 ||
                period == 11)
                return 5;

            return 4;
        }

        /// <summary>
        /// Retrieves the merchandise calendar period from the date.
        /// </summary>
        /// <param name="date">
        /// Date you wish to get the merchandise period from.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// integer
        /// </returns>
        public static int GetPeriod(DateTime date,
            bool restated = false)
        {
            return GetPeriod(GetWeek(date, restated));
        }

        /// <summary>
        /// Retrieves the merchandise calendar period from the week. 
        /// </summary>
        /// <param name="week">
        /// The week you wish to get the merchandise period from.
        /// </param>
        /// <returns>
        /// integer
        /// </returns>
        public static int GetPeriod(int week)
        {
            // Make sure the week is valid.
            ValidateWeek(week);

            // Returns period based on 4-5-4 quarter structure.
            if (week <= 4)
                return 1;

            if (week <= 9)
                return 2;

            if (week <= 13)
                return 3;

            if (week <= 17)
                return 4;

            if (week <= 22)
                return 5;

            if (week <= 26)
                return 6;

            if (week <= 30)
                return 7;

            if (week <= 35)
                return 8;

            if (week <= 39)
                return 9;

            if (week <= 43)
                return 10;

            return week <= 48 ? 11 : 12;
        }

        /// <summary>
        /// Retrieves a date range for the merchandise period based on the period number and the year.
        /// </summary>
        /// <param name="period">
        /// The merchandise period you wish to get the date range information for.
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
        public static DateRange GetPeriodDateRange(int period,
            int year,
            bool restated = false)
        {
            // Make sure it's a valid period.
            ValidatePeriod(period);

            // Get merchandise year information.
            var merchYear = GetMerchYearInfo(year);
            // Number of weeks' worth of days to skip to get to the beginning of the period.
            var weeksToSkip = 0;

            // Add the number of weeks in each period up to the period passed in to get the number of weeks to skip.
            for (var i = 1; i < period; i++)
            {
                weeksToSkip += WeeksInPeriod(i);
            }

            // Add an extra week if it's a 53 week year and restated is true.
            if (merchYear.ExtraWeek && restated)
                weeksToSkip += 1;

            var merchYearStartDate = merchYear.DateRange.StartDate;
            var weeksInPeriod = WeeksInPeriod(period);

            /* Add the number of days in the weeks to skip variable to the merch year start date to get
             * the start date of the date range. */
            var startDate = merchYearStartDate.AddDays(weeksToSkip * 7);
            // add the number of days in the period to the start date to get the end date.
            var endDate = startDate.AddDays((weeksInPeriod * 7) - 1).ToEndOfDay();

            return new DateRange
                {
                    StartDate = startDate,
                    EndDate = endDate
                };
        }

        /// <summary>
        /// Retrieves a date range for the merchandise period based on the date.
        /// </summary>
        /// <param name="date">
        /// The date you wish to get the date range information for.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// DateRange
        /// </returns>
        public static DateRange GetPeriodDateRange(DateTime date,
            bool restated = false)
        {
            var period = GetPeriod(date, restated);
            var year = GetYear(date);

            return GetPeriodDateRange(period, year, restated);
        }

        /// <summary>
        /// Returns date range from the beginning of the period to the date.
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
        public static DateRange GetPeriodToDate(DateTime date,
            bool restated = false)
        {
            var period = GetPeriod(date, restated);
            var year = GetYear(date);
            var startDate = GetPeriodDateRange(period, year, restated).StartDate;
            
            return new DateRange
            {
                StartDate = startDate,
                EndDate = date.ToEndOfDay()
            };
        }
    }
}

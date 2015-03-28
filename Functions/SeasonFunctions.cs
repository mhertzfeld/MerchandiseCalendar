using System;

namespace MerchandiseCalendar
{
    public static partial class DateFunctions
    {
        /// <summary>
        /// Retrieves the season based on the date.
        /// </summary>
        /// <param name="date">
        /// Date you wish to get the season for.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// string
        /// </returns>
        public static string GetSeason(DateTime date,
            bool restated = false)
        {
            return GetSeason(GetWeek(date, restated));
        }

        /// <summary>
        /// Retrieves the season based on the week.
        /// </summary>
        /// <param name="week">
        /// Merchandise week you wish to get the season for.
        /// </param>
        /// <returns></returns>
        public static string GetSeason(int week)
        {
            // Make sure the week is valid.
            ValidateWeek(week);

            return week <= 26 ? "Spring" : "Fall";
        }

        /// <summary>
        /// Retrieves the date range for the season based on season and year.
        /// </summary>
        /// <param name="season">
        /// The season you wish to get the date range for.
        /// </param>
        /// <param name="year">
        /// The year of the season you wish to get the date range for.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// DateRange
        /// </returns>
        public static DateRange GetSeasonDateRange(Season season,
            int year,
            bool restated = false)
        {
            // Default to Spring season, weeks 1 through 26.
            var startWeek = 1;
            var endWeek = 26;

            if (season == Season.Fall)
            {
                // Check whether the year has a 53rd week.
                var extraWeek = GetMerchYearInfo(year).ExtraWeek;

                startWeek = 27;

                /* If the year has an extra week and the time period has not been restated, end on 
                 * 53rd week, otherwise end on 52nd. */
                if (extraWeek && !restated)
                    endWeek = 53;
                else
                    endWeek = 52;
            }

            var startDate = GetWeekDateRange(startWeek, year, restated).StartDate;
            var endDate = GetWeekDateRange(endWeek, year, restated).EndDate.ToEndOfDay();

            return new DateRange
            {
                StartDate = startDate,
                EndDate = endDate
            };
        }

        /// <summary>
        /// Retrieves the date range for the season based on the date.
        /// </summary>
        /// <param name="date">
        /// The season you wish to get the date range for.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// DateRange
        /// </returns>
        public static DateRange GetSeasonDateRange(DateTime date,
            bool restated = false)
        {
            // Get the season for the date passed in.
            Season season;
            var seasonString = GetSeason(date, restated);
            var seasonParsed = Enum.TryParse(seasonString, out season);

            // Check if it's a valid season.
            if (!seasonParsed)
                throw new InvalidSeasonException(seasonString);

            var year = GetYear(date);

            return GetSeasonDateRange(season, year, restated);
        }
    }
}

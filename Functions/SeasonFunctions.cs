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
        /// <returns>
        /// string
        /// </returns>
        public static string GetSeason(DateTime date)
        {
            return GetSeason(GetWeek(date));
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
        /// <returns>
        /// DateRange
        /// </returns>
        public static DateRange GetSeasonDateRange(Season season,
            int year)
        {
            // Default to Spring season, weeks 1 through 26.
            var startWeek = 1;
            var endWeek = 26;

            if (season == Season.Fall)
            {
                startWeek = 27;
                
                if (GetMerchYearInfo(year).ExtraWeek)
                    endWeek = 53;
                else
                    endWeek = 52;
            }

            var startDate = GetWeekDateRange(startWeek, year).StartDate;
            var endDate = GetWeekDateRange(endWeek, year).EndDate.ToEndOfDay();

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
        /// <returns>
        /// DateRange
        /// </returns>
        public static DateRange GetSeasonDateRange(DateTime date)
        {
            // Get the season for the date passed in.
            Season season;
            var seasonString = GetSeason(date);
            var seasonParsed = Enum.TryParse(seasonString, out season);

            // Check if it's a valid season.
            if (!seasonParsed)
                throw new InvalidSeasonException(seasonString);

            var year = GetYear(date);

            return GetSeasonDateRange(season, year);
        }
    }
}

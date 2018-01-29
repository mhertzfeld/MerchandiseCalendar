using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MerchandiseCalendar
{
    /// <summary>
    /// Collection of functions that calculate and return information about the NRF's 4-5-4 Merchandise Calendar
    /// </summary>
    public static partial class DateFunctions
    {
        #region enum

        /// <summary>
        /// Merchandise season.
        /// </summary>
        public enum Season
        {
            /// <summary>
            /// Spring season, periods 1 - 6.
            /// </summary>
            Spring = 0,
            /// <summary>
            /// Fall Season, periods 7 - 12.
            /// </summary>
            Fall = 1
        }

        #endregion

        #region Other Methods

        private static DateTime ToEndOfDay(this DateTime date)
        {
            // Sets time to the end of the day. Useful for reporting based on date ranges.
            return date.Date
                .AddHours(23)
                .AddMinutes(59)
                .AddSeconds(59)
                .AddMilliseconds(999);
        }

        /// <summary>
        /// Returns a list of DateTime representing all calendar days between two dates.
        /// </summary>
        /// <param name="startDate">
        /// The start date.
        /// </param>
        /// <param name="endDate">
        /// The end date.
        /// </param>
        /// <returns>
        /// IEnumerable&lt;DateTime&gt;
        /// </returns>
        public static IEnumerable<DateTime> GetAllDatesBetween(DateTime startDate, DateTime endDate)
        {
            // Make sure it's a valid date range.
            ValidateDateRange(new DateRange
            {
                StartDate = startDate,
                EndDate = endDate
            });

            // Returns a list of dates between the two dates.
            for (var day = startDate.Date; day <= endDate; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        /// <summary>
        /// Returns a list of DateTime representing all calendar days between two dates.
        /// </summary>
        /// <param name="dateRange">
        /// The date range you wish to get the list of dates for.
        /// </param>
        /// <returns>
        /// IEnumerable&lt;DateTime&gt;
        /// </returns>
        public static IEnumerable<DateTime> GetAllDatesBetween(this DateRange dateRange)
        {
            ValidateDateRange(dateRange);

            return GetAllDatesBetween(dateRange.StartDate, dateRange.EndDate);
        }

        /// <summary>
        /// Retrieves the date of the same day of the merchandise calendar for the year provided. Same day is determined by the week number and day of week in the year provided. Since this is comparative, restated is set to true.
        /// </summary>
        /// <param name="date">
        /// The date being compared.
        /// </param>
        /// <param name="year">
        /// The year you wish to get the information for.
        /// </param>
        /// <param name="_Week53Options">
        /// _Week53Options
        /// </param>
        /// <returns>
        /// DateTime
        /// </returns>
        public static DateTime? GetComparisonDay(DateTime date,
            int year,
            Week53Options _Week53Options)
        {
            Int32 _Week = GetWeek(date);

            if (_Week == 53)
            {
                switch (_Week53Options)
                {
                    case Week53Options.AddWeek:

                        _Week = 1;

                        year = year + 1;

                        break;

                    case Week53Options.NonComp:

                        return null;

                    case Week53Options.SubtractWeek:

                        _Week = 52;

                        break;

                    default:

                        throw new ArgumentOutOfRangeException("_Week53Options");
                }
            }

            // Get list of dates for the same merch week from the inputted year.
            var weekRange = GetAllDatesBetween(GetWeekDateRange(GetWeek(date), year));
            // Return the date that falls on the same day of the week as the date.
            return weekRange.First(x => x.DayOfWeek == date.DayOfWeek);
        }

        /// <summary>
        /// Retrieves the day of the year
        /// </summary>
        /// <param name="date">
        /// The merchandise week you wish to get date range information for.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// int
        /// </returns>
        public static int GetDayOfYear(DateTime date)
        {
            int week = GetWeek(date);

            int dayOfWeek = ((int)date.DayOfWeek + 1);
            
            return (((week - 1) * 7) + dayOfWeek);
        }

        /// <summary>
        /// Retrieves the Merchandise Dates for the Date
        /// </summary>
        /// <param name="_DateTime">
        /// The date you wish to get the list of dates for.
        /// </param>
        /// <returns>
        /// MerchandiseDate
        /// </returns>
        public static MerchandiseDate GetMerchandiseDate(DateTime _DateTime)
        {
            int _Week = DateFunctions.GetWeek(_DateTime);

            int _Period = DateFunctions.GetPeriod(_Week);

            int _Month = (_Period + 1);

            if (_Month == 13)
            { _Month = 1; }

            Quarter _Quarter = DateFunctions.GetQuarter(_DateTime);

            MerchandiseDate _MerchandiseDate = new MerchandiseDate();
            _MerchandiseDate.Date = _DateTime;
            _MerchandiseDate.DayOfYear = DateFunctions.GetDayOfYear(_DateTime);
            _MerchandiseDate.Period = DateFunctions.GetPeriod(_Week);
            _MerchandiseDate.PeriodName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(_Month);
            _MerchandiseDate.Quarter = _Quarter.Number;
            _MerchandiseDate.QuarterName = _Quarter.Name;
            _MerchandiseDate.Week = _Week;
            _MerchandiseDate.Year = DateFunctions.GetYear(_DateTime);

            return _MerchandiseDate;
        }

        /// <summary>
        /// Retrieves all Merchandise Dates within the dateRange
        /// </summary>
        /// <param name="_DateRange">
        /// The date range you wish to get the list of dates for.
        /// </param>
        /// <returns>
        /// IEnumerable&lt;MerchandiseDate&gt;
        /// </returns>
        public static IEnumerable<MerchandiseDate> GetMerchandiseDatesBetween(DateRange _DateRange)
        {
            List<MerchandiseDate> _MerchandiseDateList = new List<MerchandiseDate>();
            
            for (DateTime _Date = _DateRange.StartDate; _Date <= _DateRange.EndDate; _Date = _Date.AddDays(1))
            {
                MerchandiseDate _MerchandiseDate = GetMerchandiseDate(_Date);

                _MerchandiseDateList.Add(_MerchandiseDate);
            }

            return _MerchandiseDateList;
        }

        /// <summary>
        /// Retrieves all Merchandise Dates between the startDate and endDate
        /// </summary>
        /// <param name="startDate">
        /// The start date.
        /// </param>
        /// <param name="endDate">
        /// The end date.
        /// </param>
        /// <returns>
        /// IEnumerable&lt;MerchandiseDate&gt;
        /// </returns>
        public static IEnumerable<MerchandiseDate> GetMerchandiseDatesBetween(DateTime startDate, DateTime endDate)
        {
            DateRange dateRange = new DateRange();
            dateRange.EndDate = endDate;
            dateRange.StartDate = startDate;

            return GetMerchandiseDatesBetween(dateRange);
        }

        /// <summary>
        /// Retrieves all Merchandise Dates within the Merchandise Year
        /// </summary>
        /// <param name="year">
        /// The year you wish to get the list of dates for.
        /// </param>
        /// <returns>
        /// IEnumerable&lt;MerchandiseDate&gt;
        /// </returns>
        public static IEnumerable<MerchandiseDate> GetMerchandiseDatesByYear(int year)
        {
            MerchYear merchYear = new MerchYear(year);

            return GetMerchandiseDatesBetween(merchYear.DateRange);
        }

        /// <summary>
        /// Retrieves the Sales Release Date of the period and year provided. Sales release day falls on the first Thursday of the period.
        /// </summary>
        /// <param name="period">
        /// The period you wish to get the Sales Release Date for.
        /// </param>
        /// <param name="year">
        /// The year you wish to get the Sales Release Date for.
        /// </param>
        /// <returns>
        /// DateTime
        /// </returns>
        public static DateTime GetSalesReleaseDay(int period, int year)
        {
            /* Gets the date range for the period, and adds 4 days to the start date to return the 
             * date for the first thursday. */
            return GetPeriodDateRange(period, year).StartDate.AddDays(4);
        }

        /// <summary>
        /// Retrieves the Sales Release Date for the period of the date provided.
        /// </summary>
        /// <param name="date">
        /// The date you wish to get the Sales Release Date for.
        /// </param>
        /// <returns>
        /// DateTime
        /// </returns>
        public static DateTime GetSalesReleaseDay(DateTime date)
        {
            var period = GetPeriod(date);
            var year = GetYear(date);

            return GetSalesReleaseDay(period, year);
        }

        /// <summary>
        /// Retrieves all Sales Release Dates for the season and year provided.
        /// </summary>
        /// <param name="season">
        /// The season you wish to get sales release dates for.
        /// </param>
        /// <param name="year">
        /// The year you wish to get sales release dates for.
        /// </param>
        /// <returns>
        /// IEnumerable&lt;DateTime&gt;
        /// </returns>
        public static IEnumerable<DateTime> GetSalesReleaseDatesForSeason(Season season, int year)
        {
            // Set the starting period depending on the season.
            var startPeriod = season == Season.Spring ? 1 : 7;
            var returnValue = new List<DateTime>();

            /* Add sales release dates for all periods from the starting period to the end of the 
             * season (start period plus 5). */
            for (var i = startPeriod; i <= (startPeriod + 5); i++)
            {
                returnValue.Add(GetSalesReleaseDay(i, year));
            }

            return returnValue;
        }

        /// <summary>
        /// Retrieves all Sales Release Dates for the year provided.
        /// </summary>
        /// <param name="year">
        /// The year you wish to get sales release dates for.
        /// </param>
        /// <returns>
        /// IEnumerable&lt;DateTime&gt;
        /// </returns>
        public static IEnumerable<DateTime> GetSalesReleaseDaysForYear(int year)
        {
            var returnValue = new List<DateTime>();

            // Get sales release dates for priods 1 through 12.
            for (var i = 1; i <= 12; i++)
            {
                returnValue.Add(GetSalesReleaseDay(i, year));
            }

            return returnValue;
        }

        #endregion

        #region Validations & Exceptions

        private static void ValidateDateRange(DateRange dateRange)
        {
            if (dateRange.StartDate > dateRange.EndDate)
                throw new InvalidDateRangeException(dateRange);
        }

        static private void ValidateWeek(int week)
        {
            if (week < 0 || week > 53)
                throw new InvalidMerchWeekException(week);
        }

        static private void ValidatePeriod(int period)
        {
            if (period < 1 || period > 12)
                throw new InvalidPeriodException(period);
        }

        static private void ValidateQuarter(int quarter)
        {
            if (quarter < 1 || quarter > 5)
                throw new InvalidQuarterException(quarter);
        }

        #endregion
    }
}

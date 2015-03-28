using System;

namespace MerchandiseCalendar
{
    /// <summary>
    /// Represents information about the quarter for a specific year.
    /// </summary>
    public class Quarter
    {
        /// <summary>
        /// Quarter number.
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// Quarter name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Quarter year.
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// Start & end date for the quarter.
        /// </summary>
        public DateRange DateRange { get; set; }
    }

    public static partial class DateFunctions
    {
        /// <summary>
        /// Retrieves information about the sales quarter based on the period number and the year.
        /// </summary>
        /// <param name="period">
        /// The period you wish to get quarter information for.
        /// </param>
        /// <param name="year">
        /// The year you wish to get quarter information for.
        /// </param>
        /// <returns>
        /// Quarter
        /// </returns>
        public static Quarter GetQuarter(int period, int year)
        {
            // Make sure the period passed in is valid.
            ValidatePeriod(period);

            // Divide the period by 3, round up to the nearest whole number.
            var quarterNumber = (int) Math.Ceiling((decimal) period/3);

            // Make sure the quarter is valid.
            ValidateQuarter(quarterNumber);

            var returnValue = new Quarter
            {
                Year = year,
                Number = quarterNumber
            };

            var dateRange = new DateRange();

            // Return different values for the rest of the object depending on what the quarter number is.
            switch (returnValue.Number)
            {
                case 1:
                    returnValue.Name = "Spring";
                    dateRange.StartDate = GetPeriodDateRange(1, year).StartDate;
                    dateRange.EndDate = GetPeriodDateRange(3, year).EndDate;
                    break;
                case 2:
                    returnValue.Name = "Summer";
                    dateRange.StartDate = GetPeriodDateRange(4, year).StartDate;
                    dateRange.EndDate = GetPeriodDateRange(6, year).EndDate;
                    break;
                case 3:
                    returnValue.Name = "Fall";
                    dateRange.StartDate = GetPeriodDateRange(7, year).StartDate;
                    dateRange.EndDate = GetPeriodDateRange(9, year).EndDate;
                    break;
                case 4:
                    returnValue.Name = "Winter";
                    dateRange.StartDate = GetPeriodDateRange(10, year).StartDate;
                    dateRange.EndDate = GetPeriodDateRange(12, year).EndDate;
                    break;
            }

            returnValue.DateRange = dateRange;

            return returnValue;
        }

        /// <summary>
        /// Retrieves information about the sales quarter based on the date.
        /// </summary>
        /// <param name="date">
        /// The date you wish to get quarter information for.
        /// </param>
        /// <param name="restated">
        /// Set to true if you want the time period adjusted forward in 53 week years for comparability to 52 week years.
        /// </param>
        /// <returns>
        /// Quarter
        /// </returns>
        public static Quarter GetQuarter(DateTime date, bool restated = false)
        {
            var period = GetPeriod(date, restated);
            var year = GetYear(date);

            return GetQuarter(period, year);
        }
    }
}

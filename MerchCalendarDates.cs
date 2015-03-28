using System;

namespace MerchandiseCalendar
{
    /// <summary>
    /// The date range for the merchandise calendar year, consisting of a start date and an end date.
    /// </summary>
    public class DateRange
    {
        /// <summary>
        /// The start date for the date range.
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// The end date for the date range.
        /// </summary>
        public DateTime EndDate { get; set; }
    }

    /// <summary>
    /// Retrieves information about the merchandise year.
    /// </summary>
    public class MerchYear
    {
        /* Set up so that the only thing that needs a public setter is the year. All other attributes
         * are calculated from the year that's set when object is instantiated. */
        private int _year;

        /// <summary>
        /// Retrieves information about the merchandise year.
        /// </summary>
        /// <param name="year">
        /// The year you wish to get the information for.
        /// </param>
        public MerchYear(int year)
        {
            _year = year;
            SetDateRange(_year);
        }

        /// <summary>
        /// Retrieves information about the merchandise year.
        /// </summary>
        public MerchYear() { }

        /* The Merchandise calendar year is a 52 week year starting in February. It's calculated by
         * taking the first full Sunday to Saturday week in February with less than four days from 
         * January in it. If, after the 52 weeks have been calculated, there are still more than three
         * days from January of the next calenday year left, an extra, 53rd week is added to the 
         * merchandise calendar. This happens every five or six years to account for the one day 
         * discrepancy between the merchandise calendar year and the normal calendar year. */

        private void SetDateRange(int year)
        {
            // Merchandise calendar starts in February.
            var firstDayOfFeb = new DateTime(year, 2, 1);

            // Integer representing the day of the week for the first of February.
            var firstFebDayOfWeek = (int)firstDayOfFeb.DayOfWeek;
            var nextYear = year + 1;

            /* Subtract days until you get to Sunday, which is the start of a merchandise calendar week.
             * This is your start date for the year, except when... */
            var startDate = firstDayOfFeb.AddDays((0 - firstFebDayOfWeek));

            /* ...February 1st falls on Thu - Sat, meaning there are more than three days from January in the
             * first week, also meaning the year before was a 53 week year. In that case, add a week 
             * to the start date. */
            if (firstFebDayOfWeek > 3)
                startDate = startDate.AddDays(7);

            /* Add 52 weeks' worth of days (364) to the start date to get the end date, taking into 
             * account the start day constitutes one of them (-1). */
            var endDate = startDate.AddDays(363);

            /* If the end date is January 27th of next year or earlier, there will be more than 3 January
             * days at the beginning of the next year and another week needs to be added to account for this. */
            if (endDate <= new DateTime(nextYear, 1, 27))
            {
                endDate = endDate.AddDays(7);
                ExtraWeek = true;
            }
            else
            {
                ExtraWeek = false;
            }

            DateRange = new DateRange
            {
                StartDate = startDate,
                EndDate = endDate.Add(new TimeSpan(0, 23, 59, 59, 999))
            };
        }

        /// <summary>
        /// The merchandise year.
        /// </summary>
        public int Year
        {
            get { return _year; }
            set
            {
                SetDateRange(value);
                _year = value;
            }
        }

        /// <summary>
        /// The date range for the merchandise year.
        /// </summary>
        public DateRange DateRange { get; private set; }

        /// <summary>
        /// Indicates whether or not the merchandise year contains 52 or 53 weeks.
        /// </summary>
        public bool ExtraWeek { get; private set; }
    }
}

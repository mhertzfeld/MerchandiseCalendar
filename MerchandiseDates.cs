using System;
using System.Collections.Concurrent;


namespace MerchandiseCalendar
{
    public class MerchandiseDates
    {
        #region Fields
        protected ConcurrentDictionary<DateTime, MerchandiseDate> _ByDateMerchandiseDateDictionary;

        protected ConcurrentDictionary<DayOfTheYear, MerchandiseDate> _ByDayOfTheYearMerchandiseDateDictionary;
        #endregion


        #region Constructor
        public MerchandiseDates()
        {
            _ByDateMerchandiseDateDictionary = new ConcurrentDictionary<DateTime, MerchandiseDate>();

            _ByDayOfTheYearMerchandiseDateDictionary = new ConcurrentDictionary<DayOfTheYear, MerchandiseDate>();
        }
        #endregion


        #region METHODS
        /// <summary>
        /// Get the Merchandise Date for the specified Day of the Year
        /// </summary>
        /// <param name="_Year">The Merchandise Year</param>
        /// <param name="_DateTime">The Date you want the Merchandise Date for.</param>
        /// <returns></returns>
        public virtual MerchandiseDate GetComparisonMerchandiseDate(Int32 _Year, DateTime _DateTime)
        {
            if (_Year < 0)
            { throw new ArgumentOutOfRangeException("Year"); }

            MerchandiseDate _MerchandiseDate = GetMerchandiseDate(_DateTime);

            return GetComparisonMerchandiseDate(_MerchandiseDate.Year, _MerchandiseDate.DayOfYear);
        }

        /// <summary>
        /// Get the Merchandise Date for the specified Day of the Year
        /// </summary>
        /// <param name="_Year">The Merchandise Year</param>
        /// <param name="_DayOfYear">The Merchandise Day of Year</param>
        /// <returns></returns>
        public virtual MerchandiseDate GetComparisonMerchandiseDate(Int32 _Year, Int32 _DayOfYear)
        {
            DayOfTheYear _DayOfTheYear = new DayOfTheYear(_Year, _DayOfYear);

            MerchandiseDate _MerchandiseDate;
            if (_ByDayOfTheYearMerchandiseDateDictionary.TryGetValue(_DayOfTheYear, out _MerchandiseDate))
            { return _MerchandiseDate; }

            AddYear(_Year);

            if (_ByDayOfTheYearMerchandiseDateDictionary.TryGetValue(_DayOfTheYear, out _MerchandiseDate))
            { return _MerchandiseDate; }

            throw new Exception("An unexpected error occured.");
        }

        /// <summary>
        /// Get the Merchandise Date for the specified date.
        /// </summary>
        /// <param name="_DateTime">Date you want the Merchandise Date for.</param>
        /// <returns></returns>
        public virtual MerchandiseDate GetMerchandiseDate(DateTime _DateTime)
        {
            MerchandiseDate _MerchandiseDate;
            if (_ByDateMerchandiseDateDictionary.TryGetValue(_DateTime, out _MerchandiseDate))
            { return _MerchandiseDate; }

            Int32 _MerchandiseYear = DateFunctions.GetYear(_DateTime);

            AddYear(_MerchandiseYear);

            if (_ByDateMerchandiseDateDictionary.TryGetValue(_DateTime, out _MerchandiseDate))
            { return _MerchandiseDate; }

            throw new Exception("An unexpected error occured.");
        }
        #endregion


        #region FUNCTIONS
        /// <summary>
        /// Adds Merchandise dates for the year specified to the Merchandise Date Dictionaries.
        /// </summary>
        /// <param name="_MerchandiseYear">The Merchandise Year to add.</param>
        protected virtual void AddYear(Int32 _MerchandiseYear)
        {
            foreach (MerchandiseDate _MerchandiseDate in DateFunctions.GetMerchandiseDatesByYear(_MerchandiseYear))
            {
                _ByDateMerchandiseDateDictionary.TryAdd(_MerchandiseDate.Date.Value, _MerchandiseDate);

                DayOfTheYear _DayOfTheYear = new DayOfTheYear(_MerchandiseDate.Year, _MerchandiseDate.DayOfYear);
                _ByDayOfTheYearMerchandiseDateDictionary.TryAdd(_DayOfTheYear, _MerchandiseDate);
            }
        }
        #endregion
    }
}
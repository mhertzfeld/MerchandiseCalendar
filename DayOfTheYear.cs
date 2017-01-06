using System;


namespace MerchandiseCalendar
{
    public class DayOfTheYear
    {
        #region Fields
        protected Int32 _DayOfYear;

        protected Int32 _Year;
        #endregion


        #region Properties
        public Int32 DayOfYear
        {
            get { return _DayOfYear; }

            protected set
            {
                if ((DayOfYear < 0) || (DayOfYear > 371))
                { throw new Exception("DayOfYear set to an Out of Range Value."); }

                _DayOfYear = value;
            }
        }

        public Int32 Year
        {
            get { return _Year; }

            protected set
            {
                if (Year < 0)
                { throw new Exception("Year set to an Out of Range Value."); }
            }
        }
        #endregion


        #region Constructor
        public DayOfTheYear(Int32 Year, Int32 DayOfYear)
        {
            this.Year = Year;

            this.DayOfYear = DayOfYear;
        }
        #endregion
    }
}
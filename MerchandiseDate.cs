using System;

namespace MerchandiseCalendar
{
    public class MerchandiseDate
    {
        public DateTime? Date;

        public Int32? DayOfYear;

        public Int32? Period;

        public String PeriodName;

        public Int32? Quarter;

        public String QuarterName;

        public Int32? Week;

        public Int32? Year;


        public MerchandiseDate()
        {
            Date = null;

            DayOfYear = null;

            Period = null;

            PeriodName = null;

            Quarter = null;

            QuarterName = null;

            Week = null;

            Year = null;
        }
    }
}
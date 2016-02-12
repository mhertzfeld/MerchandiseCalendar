using System;

namespace MerchandiseCalendar
{
    public class MerchandiseDate
    {
        //FIELDS
        DateTime? date;

        Int32 dayOfYear;

        Int32 period;

        String periodName;

        Int32 quarter;

        String quarterName;

        Int32 week;

        Int32 year;


        //PROPERTIES
        public DateTime? Date
        {
            get { return date; }

            set
            {
                if (value == default(DateTime?))
                { throw new Exception("Date cannot be set to null."); }

                date = value;
            }
        }

        public Int32 DayOfYear
        {
            get { return dayOfYear; }

            set
            {
                if ((value < 1) || (value > 371))
                { throw new Exception("DayOfYear cannot be less than 1 or greater than 371."); }

                dayOfYear = value;
            }
        }

        public Int32 Period
        {
            get { return period; }

            set
            {
                if ((value < 1) || (value > 12))
                { throw new Exception("Period cannot be less than 1 or greater than 12."); }

                period = value;
            }
        }

        public String PeriodName
        {
            get { return periodName; }

            set
            {
                if (value == default(String))
                { throw new Exception("PeriodName cannot be set to null."); }

                periodName = value;
            }
        }

        public Int32 Quarter
        {
            get { return quarter; }

            set
            {
                if ((value < 1) || (value > 4))
                { throw new Exception("Quarter cannot be less than 1 or greater than 4."); }

                quarter = value;
            }
        }

        public String QuarterName
        {
            get { return quarterName; }

            set
            {
                if (value == default(String))
                { throw new Exception("quarterName cannot be set to null."); }

                quarterName = value;
            }
        }

        public Int32 Week
        {
            get { return week; }

            set
            {
                if ((value < 1) || (value > 53))
                { throw new Exception("Week cannot be less than 1 or greater than 53."); }

                week = value;
            }
        }

        public Int32 Year
        {
            get { return year; }

            set
            {
                if ((value < 1) || (value > 9999))
                { throw new Exception("Week cannot be less than 1 or greater than 9999."); }

                year = value;
            }
        }


        //INITIALIZE
        public MerchandiseDate()
        {
            date = null;

            dayOfYear = 0;

            period = 0;

            periodName = null;

            quarter = 0;

            quarterName = null;

            week = 0;

            year = 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportControlSystem
{
    internal class PeriodType
    {
        String _period_Type;
        public System.String Period_Type
        {
            get { return _period_Type; }
            set { _period_Type = value; }
        }
    }

    internal class Period
    {
        Int32 _period_ID;
        public System.Int32 Period_ID
        {
            get { return _period_ID; }
            set { _period_ID = value; }
        }
        DateTime _start_Date;
        public System.DateTime Start_Date
        {
            get { return _start_Date; }
            set { _start_Date = value; }
        }
        DateTime _end_Date;
        public System.DateTime End_Date
        {
            get { return _end_Date; }
            set { _end_Date = value; }
        }
        Int32 _period_Type_ID;
        public System.Int32 Period_Type_ID
        {
            get { return _period_Type_ID; }
            set { _period_Type_ID = value; }
        }
        String _period_Type;
        public System.String Period_Type
        {
            get { return _period_Type; }
            set { _period_Type = value; }
        }
        internal Period(DateTime startDT, DateTime endDT, int periodTypeID)
        {
            _start_Date = startDT;
            _end_Date = endDT;
            _period_Type_ID = periodTypeID;
        }

        internal Period(Int32 id, DateTime startDT, DateTime endDT, int periodTypeID, String period_type)
        {
            _period_ID = id;
            _start_Date = startDT;
            _end_Date = endDT;
            _period_Type_ID = periodTypeID;
            _period_Type = period_type;
        }
    }
}

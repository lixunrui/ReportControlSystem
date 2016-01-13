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

        Int32 _periodDateRange;
        public System.Int32 PeriodDateRange
        {
            get { return _periodDateRange; }
            set { _periodDateRange = value; }
        }

        internal PeriodType(String typeName)
        {
            _period_Type = typeName;
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
        Boolean _period_Status;
        public System.Boolean Period_Status
        {
            set 
            { 
                if (value) // 1: true - closed
                {
                    _period_Status_Str = Constants.PeriodElements.Period_Status_Already_Closed;
                }
                else
                {
                    _period_Status_Str = Constants.PeriodElements.Period_Status_Close;
                }
                _period_Status = value; 
            }
            get { return !_period_Status; }
        }

        String _period_Status_Str;
        public System.String Period_Status_Str
        {
            get { return _period_Status_Str; }
        }

        Int32 _periodDateRange;
        public System.Int32 PeriodDateRange
        {
            get { return _periodDateRange; }
            set { _periodDateRange = value; }
        }

        internal Period()
        {

        }

        internal Period(DateTime startDT, DateTime endDT)
        {
            _start_Date = startDT;
            _end_Date = endDT;
        }

        internal Period(DateTime startDT, DateTime endDT, int periodTypeID)
        {
            _start_Date = startDT;
            _end_Date = endDT;
            _period_Type_ID = periodTypeID;
        }

        internal Period(Int32 id, DateTime startDT, DateTime endDT, int periodTypeID, String period_type, Boolean status, Int32 dateRange)
        {
            _period_ID = id;
            _start_Date = startDT;
            _end_Date = endDT;
            _period_Type_ID = periodTypeID;
            _period_Type = period_type;
            _period_Status = status;
            if (status) // 1: true - closed
            {
                _period_Status_Str = Constants.PeriodElements.Period_Status_Already_Closed;
            }
            else
            {
                _period_Status_Str = Constants.PeriodElements.Period_Status_Close;
            }

            _periodDateRange = dateRange;
        }
    }
}

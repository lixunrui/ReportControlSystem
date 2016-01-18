using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace ReportControlSystem
{
    internal class Staff
    {
        Int32 _staff_ID;
        public System.Int32 Staff_ID
        {
            set { _staff_ID = value; }
            get { return _staff_ID; }
        }

        String _name;
        public System.String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        String _employeeCode;
        public System.String EmployeeCode
        {
            get { return _employeeCode; }
            set { _employeeCode = value; }
        }

        String _taxCode;
        public System.String TaxCode
        {
            get { return _taxCode; }
            set { _taxCode = value; }
        }

        decimal _rate;
        public decimal Rate
        {
            get { return _rate; }
            set { _rate = value; }
        }

        //decimal _hours;
        //public decimal Hours
        //{
        //    get { return _hours; }
        //    set { _hours = value; }
        //}

        String _bankCode;
        public System.String BankCode
        {
            get { return _bankCode; }
            set { _bankCode = value; }
        }

        internal Staff( String name, String employeeCode, String taxCode, decimal rate)
        {
            _name = name;
            _employeeCode = employeeCode;
            _taxCode = taxCode;
            _rate = rate;
        }


        internal Staff(String name, String employeeCode, String taxCode, decimal rate, String bankCode)
            : this(name, employeeCode, taxCode, rate)
        {
           
            _bankCode = bankCode;
        }

        internal Staff(Int32 ID, String name, String employeeCode, String taxCode, decimal rate)  
            :this(name, employeeCode, taxCode, rate)
        {
            _staff_ID = ID;
        }

        internal Staff(Int32 ID, String name, String employeeCode, String taxCode, decimal rate,  String bankCode)
            : this(ID, name, employeeCode, taxCode, rate)
        {
          
            _bankCode = bankCode;
        }

    }
}

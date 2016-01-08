using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportControlSystem
{
    internal class Staff
    {
        String _name;
        public System.String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        String _phone;
        public System.String Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        internal Staff(String name)
        {
            _name = name;
        }

        internal Staff(String name, String phone):this(name)
        {
            _phone = phone;
        }
    }
}

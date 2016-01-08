using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportControlSystem
{
    internal class User
    {
        String _login_Name;
        public System.String Login_Name
        {
            get { return _login_Name; }
            set { _login_Name = value; }
        }
        String _password;
        public System.String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        internal User(string name, string pwd)
        {
            _login_Name = name;
            _password = pwd;
        }
    }
}

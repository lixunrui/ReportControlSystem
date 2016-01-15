using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportControlSystem
{
    internal class User
    {
        Int32 _iD;
        public System.Int32 ID
        {
            get { return _iD; }
            set { _iD = value; }
        }
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

        internal User()
        {

        }

        internal User(string name, string pwd)
            : this()
        {
            _login_Name = name;
            _password = pwd;
        }

        internal User(int id, string name, string password)
            : this(name, password)
        {
            _iD = id;
        }
    }
}

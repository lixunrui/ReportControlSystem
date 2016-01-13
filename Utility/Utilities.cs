using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportControlSystem
{
    // use to pass object to previous window

    internal class ObjectPassedEventArgs : EventArgs
    {
        internal Object item { get; set; }
    }
}

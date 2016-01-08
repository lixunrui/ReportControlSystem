using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportControlSystem
{
    internal class Category
    {
        String _category_Name;
        public System.String Category_Name
        {
            get { return _category_Name; }
            set { _category_Name = value; }
        }

        String _category_Description;
        public System.String Category_Description
        {
            get { return _category_Description; }
            set { _category_Description = value; }
        }

        String _category_Type;
        public System.String Category_Type
        {
            get { return _category_Type; }
            set { _category_Type = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of the Category</param>
        /// <param name="addOrMinus">true: Add; False: Minus</param>
        internal Category(string name, bool addOrMinus, string des = null)
        {
            _category_Name = name;
            _category_Description = des;
            if (addOrMinus)
            {
                _category_Type = "+";
            }
            else
            {
                _category_Type = "-";
            }
        }
    }
}

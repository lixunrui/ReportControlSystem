using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace ReportControlSystem
{
    internal class Category
    {
        Int32 _category_ID;
        public System.Int32 Category_ID
        {
            get { return _category_ID; }
            set { _category_ID = value; }
        }
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

        internal int _category_Type_bit;
        public int Category_Type_bit
        {
            get { return _category_Type_bit; }
            set 
            { 
                _category_Type_bit = value;
                if (value == 0)
                {
                    _category_Type = Constants.CategoryElements.Deduction;
                }
                else
                    _category_Type = Constants.CategoryElements.Gross;
            }
        }

        internal Category(string name, bool addOrMinus, string des = null)
        {
            _category_Name = name;
            _category_Description = des;
            if (addOrMinus)
            {
                _category_Type = Constants.CategoryElements.Gross;
                _category_Type_bit = 1;
            }
            else
            {
                _category_Type = Constants.CategoryElements.Deduction;
                _category_Type_bit = 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Name of the Category</param>
        /// <param name="addOrMinus">true: Add; False: Minus</param>
        internal Category(int id, string name, bool addOrMinus, string des = null)
            : this(name, addOrMinus, des)
        {
            _category_ID = id;
        }


        
    }
}

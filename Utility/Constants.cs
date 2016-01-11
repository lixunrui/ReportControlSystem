﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportControlSystem
{
    internal static class Constants
    {
        // staff table
        internal static class EmployeeElements
        {
            internal static String Employee_Name = "Name";
            internal static String Employee_ID = "Staff_ID";
            internal static String Employee_Code = "EmployeeCode";
            internal static String Employee_TaxCode = "TaxCode";
            internal static String Employee_Rate = "Rate";
            internal static String Employee_Hours = "Hours";
            internal static String Employee_BankCode = "BankCode";
        }

        // category table
        internal static class CategoryElements
        {
            internal static String Category_ID = "Category_ID";
            internal static String Category_Name = "Category_Name";
            internal static String Category_Description = "Category_Description";
            internal static String Category_Type = "Category_Type";

            internal static String Gross = "Gross";
            internal static String Deduction = "Deduction";
        }

        internal static class StaffCategoryElements
        {
            internal static String Staff_ID = "Staff_ID";
            internal static String Category_ID = "Category_ID";
        }

        internal static class PeriodTypesElements
        {
            internal static String Period_Type_ID = "Period_Type_ID";
            internal static String Period_Type = "Period_Type";
        }
    }
}

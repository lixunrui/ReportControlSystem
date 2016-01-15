using System;
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

        internal static class PeriodElements
        {
            internal static String Period_ID = "Period_ID";
            internal static String Start_Date = "Start_Date";
            internal static String End_Date = "End_Date";
            internal static String Period_Type_ID = "Period_Type_ID";
            internal static String Period_Type = "Period_Type";
            internal static String Period_Status = "Period_Status";

            internal static String Period_Status_Close = "Close Period";
            internal static String Period_Status_Already_Closed = "Period Closed";
            internal static String PeriodDateRange = "PeriodDateRange";
        }

        internal static class PaymentElements
        {
            internal static String PaymentID = "ID";
            internal static String Payment_StaffID = "Staff_ID";
            internal static String Payment_PeriodID = "Period_ID";
            internal static String Payment_CategoryID = "Category_ID";
            internal static String Payment_Amount = "Amount";
            internal static String Payment_Hours = "Hours";
        }

        internal static class UserElements
        {
            internal static String UserID = "ID";
            internal static String LoginName = "Login_Name";
            internal static String Password = "Password";
        }
    }
}

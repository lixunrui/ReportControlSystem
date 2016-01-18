﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace ReportControlSystem
{
    // This class is used to return the queries on how to create tables
    /*
     * This database contains the following tables:
     * 1: User table -> for login
     * 2: Category table -> for different payment types (id, category_name)
     * 3: Period table -> Record time period and period Type (id, time_rang, period_type)
     * 4: Period Type table -> for different period types: daily, weekly, fornightly, monthly, annually
     * 5: payment table -> Main table -> Record all payment details (id, staff_id, category, Amount, period_id)
     * 6: Staff table -> Record all staff info (id, name, contact etc.)
     * */

    internal static class SQLStatement
    {
#region Add 
        internal static String GetForeignKeySupportQUery()
        {
            String query = String.Empty;

            query = @"PRAGMA foreign_keys = ON;";

            return query;
        }
#endregion

#region Create Tables
        /// <summary>
        /// Create user table 
        /// ID
        /// Login_name
        /// Password (in MD5 format)
        /// </summary>
        /// <returns></returns>
        internal static String GetCreateUserTableQuery()
        {
            String query = String.Empty;

            query = @"CREATE TABLE Users (
		                ID					INTEGER PRIMARY KEY AUTOINCREMENT,
		                Login_Name			NVARCHAR(30) NOT NULL,
		                Password			NVARCHAR(100) NOT NULL
	                );";

            return query;
        }

        /// <summary>
        /// Category Table
        /// ID
        /// Name
        /// Type -> 1: plus 0: minus
        /// </summary>
        /// <returns></returns>
        internal static String GetCreateCategoryTableQuery()
        {
            String query = String.Empty;

            query = @"CREATE TABLE Category (
		                Category_ID		    INTEGER PRIMARY KEY AUTOINCREMENT,
		                Category_Name		NVARCHAR(50) NOT NULL,
                        Category_Description NVARCHAR(300),
		                Category_Type		BIT NOT NULL,
                        Deleted             bin not null default 0
	                );";

            return query;
        }

        /// <summary>
        /// Period Table
        /// ID
        /// Start Time
        /// End Time
        /// Period Type
        /// 
        /// We create a new data from the UI, only allow to select Period Type, and Start Time,
        /// the app will auto calculate the end time based on the period type
        /// </summary>
        /// <returns></returns>
        internal static String GetCreatePeriodTableQuery()
        {
            String query = String.Empty;

            query = @"CREATE TABLE Period (
		                Period_ID			INTEGER PRIMARY KEY AUTOINCREMENT,
		                Start_Date          Date,
                        End_Date            Date,
		                Period_Type_ID		INT,
                        Period_Status       bit not null default 0,
                        Deleted             bin not null default 0,
                        FOREIGN KEY (Period_Type_ID) REFERENCES Period_Type
	                );";

            return query;
        }

        /// <summary>
        /// Period Type Table
        /// ID
        /// Type: Daily, Weekly, Fortnightly, Monthly, Quarterly, Annually. 
        /// </summary>
        /// <returns></returns>
        internal static String GetCreatePeriodTypeTableQuery()
        {
            String query = String.Empty;

            query = @"CREATE TABLE PeriodType (
		                Period_Type_ID		INTEGER PRIMARY KEY AUTOINCREMENT,
		                Period_Type		    nvarchar(30) default NULL,
                        PeriodDateRange     Int not null default 1,
                        Deleted             bin not null default 0
	                );";

            return query;
        }

        /// <summary>
        /// Staff Table
        /// ID
        /// Name
        /// Cell
        /// </summary>
        /// <returns></returns>
        internal static String GetCreateStaffTableQuery()
        {
            String query = String.Empty;

            query = @"CREATE TABLE Employee (
		                Staff_ID		INTEGER PRIMARY KEY AUTOINCREMENT,
		                Name		    NVARCHAR(90) NOT NULL,
		                EmployeeCode	NVARCHAR(30) NOT NULL,
				        TaxCode		    NVARCHAR(1) NOT NULL,
				        Rate            decimal(3,3) Not Null ,
				        BankCode        NVARCHAR(30) ,
                        Deleted             bin not null default 0,
				        UNIQUE  (Staff_ID, Rate)
	                );";

            return query;
        }

        /// <summary>
        /// Payment Table
        /// ID
        /// Staff_ID
        /// Period_ID
        /// Category_ID
        /// Amount
        /// </summary>
        /// <returns></returns>
        internal static String GetCreatePaymentTableQuery()
        {
            String query = String.Empty;

            query = @"CREATE TABLE Payment (
		                ID					INTEGER PRIMARY KEY AUTOINCREMENT,
		                Staff_ID            INTEGER,
                        Hours               decimal(5,2) not null default 0.0,
                        Period_ID           INTEGER,
                        Category_ID         INTEGER,
                        Amount              decimal(12,4) NOT NULL DEFAULT 0.0,
                        FOREIGN KEY (Staff_ID) REFERENCES Staff, 
                        FOREIGN KEY (Period_ID) REFERENCES Period, 
                        FOREIGN KEY (Category_ID) REFERENCES Category 
	                );";

            return query;
        }


        internal static String GetCreateStaffCategoryTableQuery()
        {
            String query = String.Empty;

            query = @"CREATE TABLE StaffCategory
                        (
                        Staff_ID INTEGER not null, 
                        Category_ID INTEGER  not null,
                        PRIMARY KEY (Staff_ID, Category_ID),
                        FOREIGN KEY (Staff_ID) REFERENCES Staff(Staff_ID),
                        FOREIGN KEY (Category_ID) REFERENCES Category (Category_ID)
                        );";

            return query;
        }
#endregion

#region Add default data

        internal static String GetDefaultInsertUserQuery()
        {
            String query = String.Empty;

            query = @"Delete from Users;";

            query += @"delete from sqlite_sequence where name='Users';";

            query += @"INSERT INTO Users (Login_Name, Password) values ('admin','21232f297a57a5a743894a0e4a801fc3');";

            return query;
        }

        internal static String GetDefaultInsertCategoryQuery()
        {
            String query = String.Empty;

            query = @"delete from category;";

            query += @"delete from sqlite_sequence where name='category';";

            query += @"INSERT INTO category (category_name, category_type) values ('Total gross earnings', 1);";
            query += @"INSERT INTO category (category_name, category_type) values ('Less taxes', 0);";
            query += @"INSERT INTO category (category_name, category_type) values ('Child Support', 0);";
            query += @"INSERT INTO category (category_name, category_type) values ('Civil Enforcement', 0);";
            query += @"INSERT INTO category (category_name, category_type) values ('Fines in default', 0);";
            query += @"INSERT INTO category (category_name, category_type) values ('Student loan repayment', 0);";
            query += @"INSERT INTO category (category_name, category_type) values ('Kiwi Saver', 0);";
            query += @"INSERT INTO category (category_name, category_type) values ('Loans from Brothers', 0);";
            query += @"INSERT INTO category (category_name, category_type) values ('Wage paid advance', 0);";

            return query;
        }

        internal static String GetDefaultInsertPeriodTypeQuery()
        {
            String query = String.Empty;

            query = @"delete from PeriodType;";

            query += @"delete from sqlite_sequence where name='PeriodType';";

            query += @"INSERT INTO PeriodType (Period_Type,PeriodDateRange) values ('Daily', 1);";
            query += @"INSERT INTO PeriodType (Period_Type,PeriodDateRange) values ('Weekly', 7);";
            query += @"INSERT INTO PeriodType (Period_Type,PeriodDateRange) values ('Fortnightly', 14);";
            query += @"INSERT INTO PeriodType (Period_Type,PeriodDateRange) values ('Monthly', 30);";
            query += @"INSERT INTO PeriodType (Period_Type,PeriodDateRange) values ('Quarterly', 90);";
            query += @"INSERT INTO PeriodType (Period_Type,PeriodDateRange) values ('Annually',365);";

            return query;
        }


#endregion

#region Select Query
        internal static String GetUserTableQuery()
        {
            String query = String.Empty;

            query = @"select * from Users;";

            return query;
        }

        internal static String GetUserFromIDQuery(int id)
        {
            String query = String.Empty;

            query = String.Format(@"select * from Users where ID={0};", id);

            return query;
        }

        internal static String GetPeriodTypeTableQuery()
        {
            String query = String.Empty;

            query = @"select * from PeriodType where deleted = 0;";

            return query;
        }

        internal static String GetAllClosedPeriodDetailsQuery()
        {
            String query = String.Empty;

            query = @"select period.Period_ID, period.Start_Date, period.End_Date, 
period.Period_Type_ID, period.Period_Status, 
periodType.Period_Type, periodType.PeriodDateRange  
from period inner join periodtype 
on periodType.period_type_ID=period.period_type_ID 
where period_status = 1;";

            return query;
        }


        /// <summary>
        /// Return all OPEN period to be selected.
        /// </summary>
        /// <returns></returns>
        internal static String GetPeriodAndTypeTableQuery()
        {
            String query = String.Empty;

            query = @"select * from periodtype inner join period on                periodType.period_type_id = period.Period_type_ID 
where period_status=0;";

            return query;
        }

        internal static String GetPaymentDetailsFromStaffIDAndPeriodIDTableQuery(int staffID, int periodID)
        {
            String query = String.Empty;

            query = String.Format(@"select employee.name, employee.employeecode, employee.Taxcode,
employee.rate, payment.Hours,  
period.start_date, period.end_date,
category.category_name, category.category_Type, payment.amount
from 
payment inner join employee 
on employee.staff_id = payment.staff_id
inner join period
on period.period_ID = payment.period_id
inner join periodType
on periodType.period_Type_ID = period.Period_Type_ID
inner join category
on category.category_id = payment.category_id
where period.period_ID = {0} and employee.staff_id = {1};", periodID, staffID);

            return query;
        }

        internal static String GetMaxPeriodTypeIDQuery()
        {
            String query = String.Empty;

            query = @"select MAX(Period_Type_ID) from PeriodType;";

            return query;
        }

        internal static String GetDateRangeFromPeriodTypeTableWhereIDQuery(int periodTypeID)
        {
            String query = String.Empty;

            query = string.Format(@"select PeriodDateRange from PeriodType where Period_Type_ID={0};", periodTypeID);

            return query;
        }

        internal static String GetPeriodTableQuery()
        {
            String query = String.Empty;

            query = @"select Period_ID, Start_Date, End_Date, period_status, period.Period_Type_ID, periodType.Period_Type, PeriodDateRange from period left join periodType on period.Period_Type_ID = periodType.Period_Type_ID;";

            return query;
        }

        internal static String GetStaffTableQuery()
        {
            String query = String.Empty;

            query = @"select * from Employee where deleted = 0;";

            return query;
        }


        internal static String GetMaxStaffIDTableQuery()
        {
            String query = String.Empty;

            query = @"select Count(Staff_ID) from Employee;";

            return query;
        }

        internal static String GetStaffFromIDQuery(int id)
        {
            String query = String.Empty;

            query = String.Format(@"select * from Employee where {0}={1} and deleted = 0;", Constants.EmployeeElements.Employee_ID, id);

            return query;
        }

        internal static String GetCategoryTableQuery()
        {
            String query = String.Empty;

            query = @"select * from Category  where deleted = 0;";

            return query;
        }


        internal static String GetCategoryNotForStaffTableQuery(int staffID)
        {
            String query = String.Empty;

            query = string.Format(@"select * from category where category_id not in (select category_id from staffCategory where staff_ID={0} );",staffID);

            return query;
        }

        internal static String GetStaffCategoryFor(int staffID)
        {
            String query = String.Empty;

            query = String.Format(@"select * from StaffCategory where staff_id = {0};", staffID);

            return query;
        }

        internal static String GetCategoryFor(int categiryID)
        {
            String query = String.Empty;

            query = String.Format(@"select * from Category where category_id = {0};", categiryID);

            return query;
        }

        internal static String GetPaymentTableQuery()
        {
            String query = String.Empty;

            query = @"select * from Payment;";

            return query;
        }

        internal static String GetPeriodFromTypeID(int periodTypeID)
        {
            String query = String.Empty;

            query = string.Format(@"select Period_ID, Start_Date, End_Date, period_status, period.Period_Type_ID,PeriodDateRange, periodType.Period_Type from period left join periodType on period.Period_Type_ID = periodType.Period_Type_ID where period.Period_ID={0};", periodTypeID);

            return query;
        }


#endregion

#region Insert Into
        internal static String GetInsertCategoryTableQuery(Category c)
        {
            String query = String.Empty;

            query = string.Format(@"insert into Category (Category_Name, Category_Description, Category_Type ) values ('{0}','{1}',{2});", c.Category_Name, c.Category_Description, c._category_Type_bit);

            return query;
        }

        internal static String GetInsertStaffCategoryTableQuery(int staff_ID, Category c)
        {
            String query = String.Empty;

            query = string.Format(@"insert into StaffCategory (Staff_ID, Category_ID ) values ({0},{1});", staff_ID, c.Category_ID);

            return query;
        }

        internal static String GetInsertStaffTableQuery(Staff s)
        {
            String query = String.Empty;

            query = string.Format(@"insert into Employee (Name, EmployeeCode, TaxCode, Rate, BankCode ) values ('{0}','{1}', '{2}', {3},  '{4}');", s.Name, s.EmployeeCode, s.TaxCode, s.Rate, s.BankCode);

            return query;
        }

        internal static String GetInsertPeriodTableQuery(Period period)
        {
            String query = String.Empty;

            query = string.Format(@"insert into Period (start_date, end_date, period_type_Id ) values ('{0}','{1}', {2});", period.Start_Date.ToString("yyyy-MM-dd"), period.End_Date.ToString("yyyy-MM-dd"), period.Period_Type_ID);

            return query;
        }


        internal static String GetInsertPeriodTypeTableQuery(PeriodType periodType)
        {
            String query = String.Empty;

            query = string.Format(@"insert into PeriodType (Period_Type, PeriodDateRange) values ('{0}',{1});", periodType.Period_Type, periodType.PeriodDateRange);

            return query;
        }

        internal static String GetInsertPaymentTableQuery(int staffID, int periodID, int categoryID, decimal amount, decimal hours)
        {
            String query = String.Empty;

            query = String.Format("insert into payment ({0},{1},{2},{3}, {4}) values ({5},{6},{7},{8},{9});", Constants.PaymentElements.Payment_StaffID,
                Constants.PaymentElements.Payment_PeriodID,
                Constants.PaymentElements.Payment_CategoryID,
                Constants.PaymentElements.Payment_Amount,
                Constants.PaymentElements.Payment_Hours,
                staffID, periodID, categoryID, amount, hours);

            return query;
        }


#endregion

#region Delete From
        internal static String GetDeleteFromCategory(Category c)
        {
            String query = String.Empty;

            query = string.Format(@"update Category set deleted = 1 where Category_ID={0}",c.Category_ID);

            return query;
        }


        internal static String GetDeleteFromStaffCategory(int staff_ID, Category c)
        {
            String query = String.Empty;

            query = string.Format(@"Update StaffCategory set deleted = 1 where Category_ID={1} AND Staff_ID={0};", staff_ID, c.Category_ID);

            return query;
        }

        internal static String GetDeleteFromStaff(int staffID)
        {
            String query = String.Empty;

            query = string.Format("Update from StaffCategory set deleted = 1 where staff_ID={0};", staffID);

            query += string.Format("Update from Employee set deleted = 1 where Staff_ID={0};", staffID);

            return query;
        }


        internal static String GetDeleteFromUser(int userID)
        {
            String query = String.Empty;

            query = string.Format("delete from Users where ID={0};", userID);

            return query;
        }

#endregion

#region Update From
        internal static String GetUpdateFromCategory(Category c)
        {
            String query = String.Empty;

            query = string.Format(@"update Category Set Category_Name='{0}', Category_Description='{1}', Category_Type={2} where Category_ID={3}", c.Category_Name,c.Category_Description,c.Category_Type_bit,c.Category_ID);

            return query;
        }

        internal static String GetUpdateForStaff(Staff s)
        {
            String query = String.Empty;

            query = string.Format(@"update Employee Set {0}='{6}', {1}='{7}', {2}='{8}', {3}={9}, {4}='{10}' where {5}={11};", Constants.EmployeeElements.Employee_Name, Constants.EmployeeElements.Employee_Code, Constants.EmployeeElements.Employee_TaxCode, Constants.EmployeeElements.Employee_Rate, Constants.EmployeeElements.Employee_BankCode, 
                Constants.EmployeeElements.Employee_ID, 
                s.Name, s.EmployeeCode, 
                s.TaxCode, s.Rate, 
                s.BankCode, s.Staff_ID);

            return query;
        }

        //internal static String GetUpdateHoursForStaff(int id)
        //{
        //    String query = String.Empty;

        //    query = string.Format(@"update Employee Set {0}=0 where {1}={2};", Constants.EmployeeElements.Employee_Hours, 
        //        Constants.EmployeeElements.Employee_ID,
        //        id);

        //    return query;
        //}


        internal static String GetUpdatePeriodCloseFromID(int periodID)
        {
            String query = String.Empty;

            query = string.Format(@"update Period set period_status = 1 where Period_ID={0};", periodID);

            return query;
        }

        internal static String GetUserUpdateFromID(int userID, string loginName, string pwd)
        {
            String query = String.Empty;

            query = string.Format(@"update Users set {0} = '{1}', {2} = '{3}' where ID={4};", 
                Constants.UserElements.LoginName,loginName,
                Constants.UserElements.Password, pwd, 
                userID);

            return query;
        }
#endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		                Category_Type		BIT NOT NULL
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
		                Start_Time          Datetime,
                        End_Time            Datetime,
		                Period_Type_ID		INT,
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
		                Period_Type		    nvarchar(30) default NULL
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

            query = @"CREATE TABLE Staff (
		                Staff_ID			INTEGER PRIMARY KEY AUTOINCREMENT,
		                Name		        NVARCHAR(90) NOT NULL,
		                Phone		        NVARCHAR(30)
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
                        Period_ID           INTEGER,
                        Category_ID         INTEGER,
                        Amount              decimal(12,4) NOT NULL DEFAULT 0.0,
                        FOREIGN KEY (Staff_ID) REFERENCES Staff, 
                        FOREIGN KEY (Period_ID) REFERENCES Period, 
                        FOREIGN KEY (Category_ID) REFERENCES Category 
	                );";

            return query;
        }
#endregion

#region Add default data

        internal static String GetInsertUserQuery()
        {
            String query = String.Empty;

            query = @"Delete from Users;";

            query += @"INSERT INTO Users (Login_Name, Password) values ('admin','21232f297a57a5a743894a0e4a801fc3');";

            return query;
        }

        internal static String GetInsertCategoryQuery()
        {
            String query = String.Empty;

            query = @"delete from category;";

            query += @"INSERT INTO category (category_name, category_type) values ('Salary', 1);";

            return query;
        }

        internal static String GetInsertPeriodTypeQuery()
        {
            String query = String.Empty;

            query = @"delete from PeriodType;";

            query += @"INSERT INTO PeriodType (Period_Type) values ('Daily');";
            query += @"INSERT INTO PeriodType (Period_Type) values ('Weekly');";
            query += @"INSERT INTO PeriodType (Period_Type) values ('Fortnightly');";
            query += @"INSERT INTO PeriodType (Period_Type) values ('Monthly');";
            query += @"INSERT INTO PeriodType (Period_Type) values ('Quarterly');";
            query += @"INSERT INTO PeriodType (Period_Type) values ('Annually');";

            return query;
        }


#endregion

#region 
        internal static String GetUserTableQuery()
        {
            String query = String.Empty;

            query = @"select * from Users;";

            return query;
        }

        internal static String GetPeriodTypeTableQuery()
        {
            String query = String.Empty;

            query = @"select * from PeriodType;";

            return query;
        }


#endregion

    }
}

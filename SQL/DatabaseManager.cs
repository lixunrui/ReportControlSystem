using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

using System.Data;
using System.IO;
using System.Diagnostics;
using System.Data.SQLite;

namespace ReportControlSystem
{
    internal enum DataBaseStatus
    {
        NewDatabase,
        DatabaseOK,
        DatabaseError,
    };

    internal enum DataBaseName
    {
        Users,
        Category,
        Period,
        Period_Type,
        Staff,
        All,
    }

    internal class DatabaseManager
    {
        const String DEFAULT_DATABASE_NAME = @"mia.db";
        
        SQLiteConnection m_dbConnection;
       
        DataSet DS;

        SQLiteDataAdapter adapter;

        DataBaseStatus _dbStatus;

        public ReportControlSystem.DataBaseStatus DbStatus
        {
            get { return _dbStatus; }
        }

        internal DatabaseManager(String path = null, String databaseName = null)
        {
            String dbpath;
            if (databaseName == null)
            {
                dbpath = Path.Combine(path, DEFAULT_DATABASE_NAME);
            }
            else
                dbpath = Path.Combine(path, databaseName);

            // check file existence 
            if (!File.Exists(dbpath))
            {
                // if not exist, then we create a new one
                SQLiteConnection.CreateFile(dbpath);
                _dbStatus = DataBaseStatus.NewDatabase;
            }
            else
            {
                _dbStatus = DataBaseStatus.DatabaseOK;
            }

            try
            {
                m_dbConnection = new SQLiteConnection();
            }
            catch (System.Exception ex)
            {
                Debug.Print(ex.InnerException.Source);
                throw new Exception(ex.Message, ex.InnerException);
            }
            

            string connectionString = string.Format("Data Source={0};Version=3;MultipleActiveResultSets=true;", dbpath);

            m_dbConnection.ConnectionString = connectionString;

            if (_dbStatus == DataBaseStatus.NewDatabase)
            {
                InitDatabase();
            }
        }

        void InitDatabase()
        {
            LoadSQLTextFile(SQLStatement.GetCreateUserTableQuery());
            LoadSQLTextFile(SQLStatement.GetCreateStaffTableQuery());
            LoadSQLTextFile(SQLStatement.GetCreatePeriodTypeTableQuery());
            LoadSQLTextFile(SQLStatement.GetCreatePeriodTableQuery());
            LoadSQLTextFile(SQLStatement.GetCreateCategoryTableQuery());
            LoadSQLTextFile(SQLStatement.GetCreatePaymentTableQuery());
            LoadSQLTextFile(SQLStatement.GetCreateStaffCategoryTableQuery());

            LoadSQLTextFile(SQLStatement.GetForeignKeySupportQUery());

            LoadSQLTextFile(SQLStatement.GetDefaultInsertUserQuery());
            LoadSQLTextFile(SQLStatement.GetDefaultInsertCategoryQuery());
            LoadSQLTextFile(SQLStatement.GetDefaultInsertPeriodTypeQuery());
        }

        // Execute all statements that don't need any return data
        internal bool LoadSQLTextFile(String sqlTextFile)
        {
            StringReader reader = new StringReader(sqlTextFile);

            String query = ParserSQLStatement(reader);

            if (query != null)
            {
                try
                {
                    using (SQLiteCommand cmd = m_dbConnection.CreateCommand())
                    {
                        if (m_dbConnection.State != ConnectionState.Closed)
                        {
                            m_dbConnection.Close();
                        }

                        m_dbConnection.Open();

                        cmd.CommandText = query.Trim();

                        cmd.ExecuteNonQuery();

                        m_dbConnection.Close();
                    }
                    return true;
                }
                catch (System.Exception ex)
                {
                }           
            }
            return false;
        }

        /// <summary>
        /// Execute all SQL statements that have data return
        /// </summary>
        /// <param name="sqlTextFile"></param>
        /// <returns></returns>
        internal SQLiteDataReader ExecuteSQLTextFile(String sqlTextFile)
        {
            StringReader reader = new StringReader(sqlTextFile);

            SQLiteDataReader sqlReader = null;

            String query = ParserSQLStatement(reader);

            if (query != null)
            {
                try
                {
                    using (SQLiteCommand cmd = m_dbConnection.CreateCommand())
                    {
                        if (m_dbConnection.State != ConnectionState.Closed)
                        {
                            m_dbConnection.Close();
                        }

                        m_dbConnection.Open();

                        cmd.CommandText = query.Trim();

                        sqlReader = cmd.ExecuteReader();

                       // m_dbConnection.Close();
                    }
                    return sqlReader;
                }
                catch (System.Exception ex)
                {
                }           
            }
            return null;
        }


        String ParserSQLStatement(StringReader reader)
        {
            String sqlStatement;

            sqlStatement = reader.ReadToEnd();

            return sqlStatement;
        }

        // testing function
        internal Boolean CheckUser(String username, String password)
        {
            Boolean valideUser = false;

            DataTable table = GetDataTable(SQLStatement.GetUserTableQuery());
            
            if (table != null)
            {
                string filter = string.Format("Login_Name='{0}'", username);
                DataRow[] rows = table.Select(filter);

                foreach (DataRow r in rows)
                {
                    if (r["Password"].ToString().Equals(MD5PWD.MD5HashPassword(password)))
                    {
                        valideUser = true;
                        break;
                    }
                }
            }

            return valideUser;
        }

        internal DataTable GetDataTable(String query)
        {
            DataTable table;

            using (SQLiteCommand command = m_dbConnection.CreateCommand())
            {
                command.CommandText = query;

                adapter = new SQLiteDataAdapter(command);

                DS = new DataSet();

                adapter.Fill(DS);

                table = DS.Tables[0];
            }
            return table;
        }

        internal bool RestoreTable(DataBaseName dbName)
        {
            bool result = false;

            switch (dbName)
            {
                case DataBaseName.Users:
                    result = LoadSQLTextFile(SQLStatement.GetDefaultInsertUserQuery());
                    break;
                case DataBaseName.Category:
                    result = LoadSQLTextFile(SQLStatement.GetDefaultInsertCategoryQuery());
                    break;
                case DataBaseName.Period_Type:
                    result = LoadSQLTextFile(SQLStatement.GetDefaultInsertPeriodTypeQuery());
                    break;
                case DataBaseName.Period:
                    
                    break;
                case DataBaseName.Staff:
                    
                    break;
                case DataBaseName.All:
                    result = (LoadSQLTextFile(SQLStatement.GetDefaultInsertUserQuery()) &&
                            LoadSQLTextFile(SQLStatement.GetDefaultInsertCategoryQuery()) &&
                            LoadSQLTextFile(SQLStatement.GetDefaultInsertPeriodTypeQuery()));
                    break;
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.IO;


namespace ReportControlSystem
{
    internal class DatabaseManager
    {
        const String DATABASE_NAME = @"Mia_DB.db";

        SQLiteConnection conn;
        SQLiteCommand cmd;
        DataSet DS;

        internal DatabaseManager()
        {
            CreateDatabase();
        }

        void CreateDatabase()
        {
            if (!File.Exists(DATABASE_NAME))
            {
                SQLiteConnection.CreateFile(DATABASE_NAME);
            }
            String connectionString = String.Format("Data Source={0};Version=3;", DATABASE_NAME);
            conn = new SQLiteConnection(connectionString);
        }

        void LoadSQLTextFile(String sqlTextFile)
        {
            StringReader reader = new StringReader(sqlTextFile);

            String query = ParserSQLStatement(reader);

        }

        String ParserSQLStatement(StringReader reader)
        {
            String sqlStatement;

            sqlStatement = reader.ReadToEnd();

            return sqlStatement;
        }

        internal Boolean CheckUser(String username, String password)
        {
            Boolean valideUser = false;

            if (username.ToLower().Equals("mia") && password.ToLower().Equals("mia"))
            {
                valideUser = true;
            }

            return valideUser;
        }
    }
}

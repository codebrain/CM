using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace CM
{
    public abstract class SqlTestBase
    {
        protected void ExecuteReader(string sql, Action<SQLiteDataReader> readerAction)
        {
            Console.Write(sql);

            var connection = CreateDatabase();

            var query = new SQLiteCommand(sql, connection);
            var results = query.ExecuteReader();
            while (results.Read())
            {
                readerAction(results);
            }
            
            connection.Close();
        }

        protected static SQLiteConnection CreateDatabase()
        {
            var connection = new SQLiteConnection("FullUri=file::memory:");
            connection.Open();
            ExecuteNonQuery(connection, "CREATE TABLE Salesperson (SalespersonID int, Name VARCHAR(64), Age int, Salary int)");
            ExecuteNonQuery(connection, "INSERT INTO Salesperson VALUES (1,'Alice',61,140000)," +
                                        "(2,'Bob',34,44000)," +
                                        "(6,'Chris',34,40000)," +
                                        "(8,'Derek',41,52000)," +
                                        "(11,'Emmit',57,115000)," +
                                        "(16,'Fred',38,38000)");

            ExecuteNonQuery(connection, "CREATE TABLE Customer (CustomerID int, Name VARCHAR(64))");
            ExecuteNonQuery(connection, "INSERT INTO Customer VALUES (4,'George')," +
                                        "(6,'Harry')," +
                                        "(7,'Ingrid')," +
                                        "(11,'Jerry')");

            ExecuteNonQuery(connection,
                "CREATE TABLE Orders (OrderID int, OrderDate DATETIME, CustomerID int, SalespersonID int, NumberOfUnits int, CostOfUnit int)");
            ExecuteNonQuery(connection, "INSERT INTO Orders VALUES (3,'2013-01-17',4,2,4,400)," +
                                        "(6,'2013-02-07',4,1,1,600)," +
                                        "(10,'2013-03-04',7,6,2,300)," +
                                        "(17,'2013-03-15',6,1,5,300)," +
                                        "(25,'2013-04-19',11,11,7,300)," +
                                        "(34,'2013-04-22',11,11,100,26)," +
                                        "(57,'2013-07-12',7,11,14,11)");
            return connection;
        }

        protected List<string> GetNames(string sql)
        {
            var names = new List<string>();
            ExecuteReader(sql, reader => names.Add(reader["Name"].ToString()));
            return names;
        }

        private static void ExecuteNonQuery(SQLiteConnection connection, string sql)
        {
            var command = new SQLiteCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
using System.Collections.Generic;
using System.Data.SQLite;
using NUnit.Framework;

namespace CM
{
    [TestFixture]
    public class Requirement10 : SqlTestBase
    {
        [Test]
        [Ignore("SQLite doesn't support ROW_NUMBER(), so this only works in SQL Server")]
        public void TestGetNameOfPersonWith3rdHighestSalary()
        {
            var names = GetData("WITH OrderedSalary AS (SELECT Name, ROW_NUMBER() OVER (ORDER BY Salary DESC) AS 'RowNumber' FROM Salesperson) SELECT Name FROM OrderedSalary WHERE RowNumber = 3");
            Assert.AreEqual(1, names.Count);
            Assert.AreEqual("Derek", names[0]);
        }

        [Test]
        public void TestCreateOrderRollupTable()
        {
            var database = CreateDatabase();

            var createTable = new SQLiteCommand("CREATE TABLE BigOrders (CustomerID int, TotalOrderValue int)", database);
            createTable.ExecuteNonQuery();

            var populateTable = new SQLiteCommand("INSERT INTO BigOrders SELECT CustomerID, SUM(NumberOfUnits * CostOfUnit) as 'TotalOrderValue' FROM Orders GROUP BY CustomerID HAVING SUM(NumberOfUnits * CostOfUnit) > 1000", database);
            populateTable.ExecuteNonQuery();

            var results = new Dictionary<int, int>();
            var query = new SQLiteCommand("SELECT * FROM BigOrders ORDER BY TotalOrderValue DESC", database);
            var reader = query.ExecuteReader();
            while (reader.Read())
            {
                results.Add(reader.GetInt32(0), reader.GetInt32(1));
            }
            database.Close();

            Assert.AreEqual(4700, results[11]);
            Assert.AreEqual(2200, results[4]);
            Assert.AreEqual(1500, results[6]);
        }

        [Test]
        [Ignore("SQLite doesn't support YEAR() or MONTH() operators, so this only works in SQL Server")]
        public void TestGetTotalOrdersGroupedByDate()
        {
            var database = CreateDatabase();

            var results = new Dictionary<string, int>();
            var query = new SQLiteCommand("SELECT CAST(YEAR(OrderDate) AS VARCHAR(4)) + '-' + CAST(MONTH(OrderDate) AS VARCHAR(2)) AS YearMonth, SUM(NumberOfUnits * CostOfUnit) AS TotalOrderValue FROM Orders GROUP BY CAST(YEAR(OrderDate) AS VARCHAR(4)) + '-' + CAST(MONTH(OrderDate) AS VARCHAR(2)) ORDER BY YearMonth DESC", database);
            var reader = query.ExecuteReader();
            while (reader.Read())
            {
                results.Add(reader[0].ToString(), reader.GetInt32(1));
            }
            database.Close();

            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(154, results["07-2013"]);
            Assert.AreEqual(4700, results["04-2013"]);
            Assert.AreEqual(2100, results["03-2013"]);
            Assert.AreEqual(600, results["02-2013"]);
            Assert.AreEqual(1600, results["01-2013"]);
        }

        [Test]
        public void TestGetTotalOrdersGroupedByDateSqliteVersion()
        {
            var database = CreateDatabase();

            var results = new Dictionary<string, int>();
            var query = new SQLiteCommand("SELECT strftime(\"%m-%Y\", OrderDate) as 'YearMonth', SUM(NumberOfUnits * CostOfUnit) AS TotalOrderValue FROM Orders GROUP BY strftime(\"%m-%Y\", OrderDate) ORDER BY YearMonth DESC", database);
            var reader = query.ExecuteReader();
            while (reader.Read())
            {
                results.Add(reader[0].ToString(), reader.GetInt32(1));
            }
            database.Close();

            Assert.AreEqual(5, results.Count);
            Assert.AreEqual(154, results["07-2013"]);
            Assert.AreEqual(4700, results["04-2013"]);
            Assert.AreEqual(2100, results["03-2013"]);
            Assert.AreEqual(600, results["02-2013"]);
            Assert.AreEqual(1600, results["01-2013"]);
        }
    }
}

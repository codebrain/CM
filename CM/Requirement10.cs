using System;
using System.Collections.Generic;
using System.Data.SQLite;
using NUnit.Framework;

namespace CM
{
    [TestFixture]
    public class Requirement10 : SqlTestBase
    {
        [Test]
        public void TestGetNameOfPersonWith3rdHighestSalary()
        {
            var names = GetNames("WITH OrderedSalary AS (SELECT Name, ROW_NUMBER() OVER (ORDER BY Salary DESC) AS 'RowNumber' FROM Salesperson) SELECT Name FROM OrderedSalary  WHERE RowNumber = 3");
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
    }
}

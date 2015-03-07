using System.Collections.Generic;
using NUnit.Framework;

namespace CM
{
    [TestFixture]
    public class Requirement9 : SqlTestBase
    {
        [Test]
        public void TestGetNamesOfSalesPeopleThatHaveOrderWithGeorge()
        {
            var names = new List<string>();
            ExecuteReader("SELECT DISTINCT(Name) FROM Salesperson " +
                          "INNER JOIN Orders " +
                          "ON Salesperson.Salespersonid = Orders.Salespersonid " +
                          "AND Orders.Customerid = 4", 
                          reader => names.Add(reader["Name"].ToString()));

            Assert.AreEqual(2, names.Count);
            Assert.Contains("Bob", names);
            Assert.Contains("Alice", names);
        }

        [Test]
        public void TestGetNamesOfSalesPeopleThatDoNotHaveOrderWithGeorge()
        {
            var names = new List<string>();
            ExecuteReader("SELECT DISTINCT(Name) FROM Salesperson " +
                          "INNER JOIN Orders " +
                          "ON Salesperson.Salespersonid = Orders.Salespersonid " +
                          "AND Orders.SalespersonID NOT IN (SELECT SalespersonID FROM Orders Where CustomerID = 4)", 
                          reader => names.Add(reader["Name"].ToString()));

            Assert.AreEqual(2, names.Count);
            Assert.Contains("Chris", names);
            Assert.Contains("Emmit", names);
        }
        [Test]
        public void TestGetNamesOfSalesPeopleThatHaveMoreThanOneOrder()
        {
            var names = new List<string>();
            ExecuteReader("SELECT Name FROM Salesperson " +
                          "INNER JOIN Orders " +
                          "ON Salesperson.Salespersonid = Orders.Salespersonid " +
                          "GROUP BY SalesPerson.Name " +
                          "HAVING COUNT(*) > 1", 
                          reader => names.Add(reader["Name"].ToString()));

            Assert.AreEqual(2, names.Count);
            Assert.Contains("Alice", names);
            Assert.Contains("Emmit", names);
        }
    }
}

#Contact Details

**Stuart Cam's Application**

Email: stuart@codebrain.com.au

Mobile: 04 20 456 575

LinkedIn: https://au.linkedin.com/in/codebrain

#Solution

##Automated Build

This GIT repo is connected to AppVeyor - a continuous deployment system. On check in:

1. Nuget packages are restored
2. Solution is built
3. C# NUnit tests are run
  1. Unit tests
  2. SQLite database tests
4. JavaScript QUnit tests are run

The current status is:

[![Build status](https://ci.appveyor.com/api/projects/status/445j3ejkohbutaom)](https://ci.appveyor.com/project/codebrain26434/cm)

If the build is passing then the code passes the tests (except for *some* SQL aggregate questions as noted below).

##Requirements

###Requirement 1

https://github.com/codebrain/CM/blob/master/CM/Requirement1.cs

###Requirement 2

https://github.com/codebrain/CM/blob/master/CM/Requirement2.cs

I decided to take a simple brute force approach here as the requirements are fairly loose. The solution could be more CPU efficient (at the cost of a larger memory footprint) by utilising lookup tables for prime numbers and pre-computed results.

###Requirement 3

https://github.com/codebrain/CM/blob/master/CM/Requirement3.cs

I started the solution using a simple Scalene algorithm then later improved with Isosceles and Equilateral specific algorithms. as the code suggests I didn't bother implementing "special" right-angle triangles as they are uncommmon.

###Requirement 4

https://github.com/codebrain/CM/blob/master/CM/Requirement4.cs

###Requirement 5

https://github.com/codebrain/CM/blob/master/CM/Requirement5.js

###Requirement 6

https://github.com/codebrain/CM/blob/master/CM/Requirement6.js

A interesting question, that has a number of solutions, each with their own quirks.
I've implemented 3 different versions here.

###Requirement 7

https://github.com/codebrain/CM/blob/master/CM/Requirement7.js

###Requirement 8

https://github.com/codebrain/CM/blob/master/CM/Requirement8.js
https://github.com/codebrain/CM/blob/master/CM/Requirement8.html

I decided to solve the problem using Angular.

I uploaded the sample JSON file to https://api.myjson.com/bins/1cqjf to be able to retrieve it via a simple GET request for testing purposes.

###Requirement 9

https://github.com/codebrain/CM/blob/master/CM/Requirement9.cs

I construct an in-memory SQLite database and verify the queries against the in-memory database.


| Question | Solution |
|----------------------------------------------------------------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Return the names of all salespeople that have an order with George | SELECT DISTINCT(Name) FROM Salesperson INNER JOIN Orders ON Salesperson.Salespersonid = Orders.Salespersonid AND Orders.Customerid = 4 |
| Return the names of all salespeople that do not have any order with George | SELECT DISTINCT(Name) FROM Salesperson INNER JOIN Orders ON Salesperson.Salespersonid = Orders.Salespersonid AND Orders.SalespersonID NOT IN (SELECT SalespersonID FROM Orders Where CustomerID = 4) |
| Return the names of salespeople that have 2 or more orders. | SELECT Name FROM Salesperson INNER JOIN Orders ON Salesperson.Salespersonid = Orders.Salespersonid GROUP BY SalesPerson.Name HAVING COUNT(*) > 1 |


###Requirement 10

https://github.com/codebrain/CM/blob/master/CM/Requirement10.cs

I construct an in-memory SQLite database and verify the queries against the in-memory database.

The problem is that SQLite does not support certain aggregate and date operations available in SQL 2012, so I was unable to verify all of the queries.


| Question | Solution |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Return the name of the salesperson with the 3rd highest salary. | WITH OrderedSalary AS (SELECT Name, ROW_NUMBER() OVER (ORDER BY Salary DESC) AS 'RowNumber' FROM Salesperson) SELECT Name FROM OrderedSalary WHERE RowNumber = 3 |
| Create a new roll­up table BigOrders(where columns are CustomerID, TotalOrderValue), and insert into that table customers whose total Amount across all orders is greater than 1000 | CREATE TABLE BigOrders (CustomerID int, TotalOrderValue int)<br/>GO<br/>INSERT INTO BigOrders SELECT CustomerID, SUM(NumberOfUnits \* CostOfUnit) as 'TotalOrderValue' FROM Orders GROUP BY CustomerID HAVING SUM(NumberOfUnits \* CostOfUnit) > 1000<br/>GO<br/>SELECT \* FROM BigOrders ORDER BY TotalOrderValue DESC<br/>GO<br/>|
| Return the total Amount of orders for each month, ordered by year, then month (both in descending order) | SELECT CAST(YEAR(OrderDate) AS VARCHAR(4)) + '-' + CAST(MONTH(OrderDate) AS VARCHAR(2)) AS YearMonth, SUM(NumberOfUnits * CostOfUnit) AS TotalOrderValue FROM Orders GROUP BY CAST(YEAR(OrderDate) AS VARCHAR(4)) + '-' + CAST(MONTH(OrderDate) AS VARCHAR(2)) ORDER BY YearMonth DESC |

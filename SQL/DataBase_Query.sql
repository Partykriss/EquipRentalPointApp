/****************************************************************************
************************   T R A N S A C T - S Q L   ************************
************************   C O U R S E     W O R K   ************************
*****************************************************************************
*****  Practic III  ****       QUERY PROCEDURE       ************************
****************************************************************************/

USE EquipRentalPointDB
GO

--Ќайти человека, который имеет максимальную начисленную за аренду сумму в категории, заданной пользователем.
CREATE PROC spCourseQueryA
	@Category nvarchar(50)
AS
WITH RentalStat AS
(SELECT c.ID ClientID, c.FullName ClientName, e.Title Equipment, cat.Title Category, e.Price*DATEDIFF(d, r.DateBegin, r.DateEnd) Total
	FROM Clients c JOIN Rentals r ON c.ID = r.ClientID
	JOIN RentEquip re ON r.ID = re.RentalID
	JOIN Equipments e ON re.EquipID = e.ID
	JOIN EquipCategory ec ON e.ID = re.EquipID
	JOIN Categories cat ON ec.CategoryID = cat.ID
	WHERE re.EquipID = ec.EquipID AND cat.Title = @Category)
SELECT TOP 1 Category, ClientName, SUM(Total) TotalSum
	FROM RentalStat
	GROUP BY Category, ClientName
	ORDER BY TotalSum DESC

--Ќайти категорию с максимальным количеством выдачи и в ней вывести 2 самых не попул€рных товара.
CREATE VIEW courseQueryB AS (
SELECT TOP 2 
	 c.Title TopCategory, e.Title Equipment, COUNT(re.RentalID) RentalCount
	FROM RentEquip re JOIN Equipments e ON re.EquipID = e.ID 
	JOIN EquipCategory ec ON ec.EquipID = e.ID JOIN Categories c ON ec.CategoryID = c.ID
	WHERE ec.CategoryID = (SELECT CategoryID FROM (SELECT TOP 1 c.ID CategoryID, c.Title, COUNT(re.RentalID) RentalCount FROM RentEquip re 
	JOIN Equipments e ON re.EquipID = e.ID JOIN EquipCategory ec ON e.ID = ec.EquipID JOIN Categories c ON ec.CategoryID = c.ID
	GROUP BY c.ID, c.Title ORDER BY RentalCount DESC) TopCategory) 
	GROUP BY e.Title, c.Title
	ORDER BY RentalCount)

CREATE PROC spCourseQueryB
AS
	SELECT * FROM courseQueryB

--¬ывести людей, которые арендовали строго 3 товара из 6 категорий.
CREATE VIEW CourseQueryC AS (
SELECT ClientName, ClientPhone FROM
(SELECT c.ID, c.FullName ClientName, c.Phone ClientPhone, COUNT(DISTINCT re.EquipID) Equips, COUNT(DISTINCT ec.CategoryID) Cats
	FROM Clients c JOIN Rentals r ON c.ID = r.ClientID
	JOIN RentEquip re ON r.ID = re.RentalID	JOIN Equipments e ON re.EquipID = e.ID
	JOIN EquipCategory ec ON e.ID = ec.EquipID JOIN Categories cat ON ec.CategoryID = cat.ID 
	GROUP BY c.ID, c.FullName, c.Phone) inner_table
	WHERE Equips = 3 AND Cats = 6)

CREATE PROC spCourseQueryC
AS
	SELECT * FROM CourseQueryC

--¬ывести количество фактов выдачи инвентар€ за период заданный пользователем
CREATE PROC spCourseQueryD
	@DateBegin date,
	@DateEnd date = Null
AS
IF @DateEnd IS NULL
	SET @DateEnd = DATEADD(m, 1, @DateBegin)

SELECT RentalID, FullName, Phone, SUM(Price) SumPrice, DateBegin, DateEnd, Payed, Total, ToPay FROM
(SELECT r.ID RentalID, c.FullName, c.Phone, e.Price, r.DateBegin, r.DateEnd,
	r.Payed, r.Total, r.Total - r.Payed ToPay
	FROM Clients c JOIN Rentals r ON c.ID = r.ClientID AND r.DateBegin BETWEEN @DateBegin AND @DateEnd
	JOIN RentEquip re ON r.ID = re.RentalID	JOIN Equipments e ON re.EquipID = e.ID
	JOIN EquipCategory ec ON e.ID = ec.EquipID JOIN Categories cat ON ec.CategoryID = cat.ID) queryD_table
	GROUP BY RentalID, FullName, Phone, DateBegin, DAteEnd, Payed, Total, ToPay

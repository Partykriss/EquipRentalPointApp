/****************************************************************************
************************   T R A N S A C T - S Q L   ************************
************************   C O U R S E     W O R K   ************************
*****************************************************************************
*****  Practic IV  *****        PROC FOR APP         ************************
****************************************************************************/

USE EquipRentalPointDB
GO

--создаем процедуру для вывода всех категорий
CREATE PROC spGetAllCategories
AS
	SELECT ID, Title 
		FROM Categories
		ORDER BY ID

-- создаем процедуру для вывода всех инвентарей
CREATE PROC spGetAllEquipments
AS
	SELECT ID, Title, Price
		FROM Equipments
		ORDER BY ID

--создаем процедуру для вывода всех категорий заданного инвентаря
CREATE PROC spGetCategoriesByEquipID
	@EquipID int
AS
	SELECT ID, Title
		FROM Categories c JOIN EquipCategory ec ON c.ID = ec.CategoryID
		WHERE ec.EquipID = @EquipID

--создаем процудуру для вывода всех зарегистрированных клиентов
CREATE PROC spGetAllClients
AS
	SELECT ID, FullName, Phone
		FROM Clients
		ORDER BY ID

--создаем процедуру для вывода всех неоплаченных полностью записей аренды
CREATE PROC spGetAllRentalsNotPayed
AS
	SELECT *
		FROM Rentals
		WHERE is_paid = 0

--создаем процедуру для вывода информации о клиенте по заданному ID
CREATE PROC spGetClientByClientID
	@ClientID int
AS
	SELECT *
		FROM Clients
		WHERE ID = @ClientID

--создаем процедуру для вывода всех предметов инвентаря включенных в арендную запись
CREATE PROC spGetEquipmentsByRentalID
	@RentalID int
AS
	SELECT *
		FROM RentEquip re JOIN Equipments e ON re.EquipID = e.ID
		WHERE re.RentalID = @RentalID
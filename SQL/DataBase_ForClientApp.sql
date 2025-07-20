/****************************************************************************
************************   T R A N S A C T - S Q L   ************************
************************   C O U R S E     W O R K   ************************
*****************************************************************************
*****  Practic IV  *****        PROC FOR APP         ************************
****************************************************************************/

USE EquipRentalPointDB
GO

--������� ��������� ��� ������ ���� ���������
CREATE PROC spGetAllCategories
AS
	SELECT ID, Title 
		FROM Categories
		ORDER BY ID

-- ������� ��������� ��� ������ ���� ����������
CREATE PROC spGetAllEquipments
AS
	SELECT ID, Title, Price
		FROM Equipments
		ORDER BY ID

--������� ��������� ��� ������ ���� ��������� ��������� ���������
CREATE PROC spGetCategoriesByEquipID
	@EquipID int
AS
	SELECT ID, Title
		FROM Categories c JOIN EquipCategory ec ON c.ID = ec.CategoryID
		WHERE ec.EquipID = @EquipID

--������� ��������� ��� ������ ���� ������������������ ��������
CREATE PROC spGetAllClients
AS
	SELECT ID, FullName, Phone
		FROM Clients
		ORDER BY ID

--������� ��������� ��� ������ ���� ������������ ��������� ������� ������
CREATE PROC spGetAllRentalsNotPayed
AS
	SELECT *
		FROM Rentals
		WHERE is_paid = 0

--������� ��������� ��� ������ ���������� � ������� �� ��������� ID
CREATE PROC spGetClientByClientID
	@ClientID int
AS
	SELECT *
		FROM Clients
		WHERE ID = @ClientID

--������� ��������� ��� ������ ���� ��������� ��������� ���������� � �������� ������
CREATE PROC spGetEquipmentsByRentalID
	@RentalID int
AS
	SELECT *
		FROM RentEquip re JOIN Equipments e ON re.EquipID = e.ID
		WHERE re.RentalID = @RentalID
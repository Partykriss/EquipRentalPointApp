/****************************************************************************
************************   T R A N S A C T - S Q L   ************************
************************   C O U R S E     W O R K   ************************
*****************************************************************************
*****  Practic II  *****      INSERT PROCEDURE       ************************
****************************************************************************/

USE EquipRentalPointDB
GO

--1. �������� ��������� ��� ���������� ��������� ��������� � ���� ������
CREATE PROC spAddCategory
	@Title nvarchar(50)
AS
SET NOCOUNT ON

INSERT Categories
VALUES
(@Title)

--2. ������� ��������� ��� ���������� ������� ������� � ��������� ��������� ���������
CREATE PROC spAddEquipCategory
	@EquipID int,
	@CategoryID int
AS 
SET NOCOUNT ON
--����� ���� ���������� ����������� ��������
IF @EquipID <= 0 OR @CategoryID <= 0
	RETURN

INSERT EquipCategory
VALUES
(@EquipID, @CategoryID)

--3. �������� ��������� ��� ���������� ��������� � ��������� �� ���� ��������� ����� ID
CREATE PROC spAddEquipment
	@Title nvarchar(50),
	@Price int,
	@Category1ID int,
	@Category2ID int = 0,
	@Category3ID int = 0
AS
SET NOCOUNT ON

DECLARE @EquipID int

INSERT Equipments
VALUES
(@Title, @Price)

SET @EquipID = @@IDENTITY
--������ �� 3� ��������� ���������
EXEC spAddEquipCategory @EquipID, @Category1ID
EXEC spAddEquipCategory @EquipID, @Category2ID
EXEC spAddEquipCategory @EquipID, @Category3ID

--4. ��������� ��������� ��� ���������� �������� � ���� ������
CREATE PROC spAddClient
	@FullName nvarchar(100),
	@Phone char(11)
AS
SET NOCOUNT ON

INSERT Clients
VALUES
(@FullName, @Phone)

--5. ������� ��������� ��� ���������� ������� � ������� RentEquip, 
--   ������� ����� �������������� ��� �������� ������ �� ������
CREATE PROC spAddRentEquip
	@RentalID int,
	@EquipID int
AS
SET NOCOUNT ON
--����� ���� ���������� ����������� ��������
IF @EquipID <= 0 OR @RentalID <= 0
	RETURN

INSERT RentEquip
VALUES
(@RentalID, @EquipID)

--6. ������� ��������� ��� �������� ������� �� ������������ ���������.
CREATE PROC spAddPayment
	@RentalID int,
	@Amount money,
	@PaymentDate date,
	@ErrorMessage nvarchar(100) OUTPUT
AS
SET NOCOUNT ON
--��������� �� ������������� ����� �������
IF @Amount <= 0
BEGIN
	SET @ErrorMessage = '���������� ��������� ������������� ������!'
	RAISERROR (@ErrorMessage, 10, 1)
	RETURN
END
--���� ���� �� ��������, ������ ������ ���������� �������
IF @PaymentDate IS NULL
	SET @PaymentDate = GETDATE()
--��������� �� ��������� �� ��������� ����� ���������
DECLARE @Diff money = (SELECT Total - Payed FROM Rentals WHERE ID = @RentalID)
IF @Diff < @Amount
BEGIN
	SET @ErrorMessage = 
		'������ ��������� ����������� ����� � ������, ��������� �������� '
		+ CAST(@Diff as nvarchar)
	RAISERROR (@ErrorMessage, 10, 1)
	RETURN
END
--��������� ������ � ������� � �������
INSERT Payments
VALUES
(@RentalID, @PaymentDate, @Amount)
--��������� ��������� ����� � ��� �������������� ������
UPDATE Rentals SET Payed = Payed + @Amount WHERE ID = @RentalID

--7. ������� ��������� ��� ���������� ������ �� ������ �� 5 ������� �� ���������
--   ������� ����������� ������ �� ����� ����������� ����������
CREATE PROC spAddRental
	@ClientID int,
	@Equip1ID int,
	@Equip2ID int = 0,
	@Equip3ID int = 0,
	@Equip4ID int = 0,
	@Equip5ID int = 0,
	@DateBegin date,
	@DateEnd date,
	@Payment money,
	@ErrorMessage nvarchar(100) OUTPUT

AS
SET NOCOUNT ON

DECLARE @RentalID int,
		@Total money,
		@Payed money,
		@DateDiff int
--���� ���� ��������� � ������� ����� � ���� ������, �� ������� 1 ����,
--���� ���� �������� ������ ����� ������, �� ������� �� ��������� � ���������� �� ������
SET @DateDiff = DATEDIFF(d, @DateBegin, @DateEnd)
IF @DateDiff = 0
	SET @DateDiff = 1
ELSE IF @DateDiff < 0
BEGIN
	SET @ErrorMessage = '������! ���� �������� ������ ����� ������!'
	RAISERROR (@ErrorMessage, 10, 3)
	RETURN
END
--������� ���������� �� ������ � ������� Rentals
INSERT Rentals
(ClientID, DateBegin, DateEnd)
VALUES
(@ClientID, @DateBegin, @DateEnd)

SET @RentalID = @@IDENTITY
--������� �� 5 ��������� ��������� � ������� �������
EXEC spAddRentEquip @RentalID, @Equip1ID
EXEC spAddRentEquip @RentalID, @Equip2ID
EXEC spAddRentEquip @RentalID, @Equip3ID
EXEC spAddRentEquip @RentalID, @Equip4ID
EXEC spAddRentEquip @RentalID, @Equip5ID
--��������� ����� � ������
SET @Total = (SELECT SUM(e.Price) * @DateDiff
					FROM Rentals r JOIN RentEquip re ON r.ID = re.RentalID 
					JOIN Equipments e ON re.EquipID = e.ID
					WHERE r.ID = @RentalID)
--������� ����� � ������ ������ ������
UPDATE Rentals SET Total = @Total WHERE ID = @RentalID
--����� ���� �������� � ����� ���� ������ ��������� ����� � ������
IF @Total < @Payment
BEGIN
	SET @ErrorMessage = '������ ��������� ����� � ������ ' 
		+ CAST (@Total AS nvarchar(12)) + ', �������������� ������'
	DELETE FROM RentEquip WHERE RentalID = @RentalID
	DELETE FROM Rentals WHERE ID = @RentalID
	RAISERROR (@ErrorMessage, 10, 3)
	RETURN
END
--����� ���� �������� ���� ������ ������ �������� ����� �� 1 ����
IF @Payment < @Total/@DateDiff
BEGIN
	SET @ErrorMessage = '������ ������������. ����������� ����� � ������ ' 
		+ CAST (@Total/@DateDiff AS nvarchar(12)) + ', �������������� ������'
	DELETE FROM RentEquip WHERE RentalID = @RentalID
	DELETE FROM Rentals WHERE ID = @RentalID
	RAISERROR (@ErrorMessage, 10, 3)
	RETURN
END
--���������� ������ ���������� � ������
EXEC spAddPayment @RentalID, @Payment, @DateBegin, @ErrorMessage OUTPUT
/****************************************************************************
************************   T R A N S A C T - S Q L   ************************
************************   C O U R S E     W O R K   ************************
*****************************************************************************
*****  Practic II  *****      INSERT PROCEDURE       ************************
****************************************************************************/

USE EquipRentalPointDB
GO

--1. Создадим процедуру для добавления категорий инвентаря в базу данных
CREATE PROC spAddCategory
	@Title nvarchar(50)
AS
SET NOCOUNT ON

INSERT Categories
VALUES
(@Title)

--2. Создаем процедуру для заполнения смежной таблицы с указанием категорий инвентаря
CREATE PROC spAddEquipCategory
	@EquipID int,
	@CategoryID int
AS 
SET NOCOUNT ON
--Выход если отправлены некорретные значения
IF @EquipID <= 0 OR @CategoryID <= 0
	RETURN

INSERT EquipCategory
VALUES
(@EquipID, @CategoryID)

--3. Создадим процедуру для добавления инвентаря с указанием до трех категорий через ID
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
--Вносим до 3х категорий инвентаря
EXEC spAddEquipCategory @EquipID, @Category1ID
EXEC spAddEquipCategory @EquipID, @Category2ID
EXEC spAddEquipCategory @EquipID, @Category3ID

--4. Создадаем процедуру для добавления клиентов в базу данных
CREATE PROC spAddClient
	@FullName nvarchar(100),
	@Phone char(11)
AS
SET NOCOUNT ON

INSERT Clients
VALUES
(@FullName, @Phone)

--5. Создаем процедуру для добавления записей в таблицу RentEquip, 
--   которая будет использоваться при создании заявки на аренду
CREATE PROC spAddRentEquip
	@RentalID int,
	@EquipID int
AS
SET NOCOUNT ON
--Выход если отправлены некорретные значения
IF @EquipID <= 0 OR @RentalID <= 0
	RETURN

INSERT RentEquip
VALUES
(@RentalID, @EquipID)

--6. Создаем процедуру для внесения платежа за арендованный инвентарь.
CREATE PROC spAddPayment
	@RentalID int,
	@Amount money,
	@PaymentDate date,
	@ErrorMessage nvarchar(100) OUTPUT
AS
SET NOCOUNT ON
--Проверяем на положительную сумму платежа
IF @Amount <= 0
BEGIN
	SET @ErrorMessage = 'Необходимо совершить положительный платеж!'
	RAISERROR (@ErrorMessage, 10, 1)
	RETURN
END
--Если дата не передана, значит платеж происходит сегодня
IF @PaymentDate IS NULL
	SET @PaymentDate = GETDATE()
--Проверяем не привешает ли внесенная сумма требуемую
DECLARE @Diff money = (SELECT Total - Payed FROM Rentals WHERE ID = @RentalID)
IF @Diff < @Amount
BEGIN
	SET @ErrorMessage = 
		'Платеж превышает необходимую сумму к оплате, требуется оплатить '
		+ CAST(@Diff as nvarchar)
	RAISERROR (@ErrorMessage, 10, 1)
	RETURN
END
--Добавляем данные о платеже в таблицу
INSERT Payments
VALUES
(@RentalID, @PaymentDate, @Amount)
--Добавляем внесенную сумму к уже сформированной заявке
UPDATE Rentals SET Payed = Payed + @Amount WHERE ID = @RentalID

--7. Создаем процедуру для оформления заявки по аренде до 5 позиций из инвентаря
--   включая минимальный платеж за сутки пользования инвентарем
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
--Если даты совпадают и возврат будет в день аренды, то разница 1 день,
--если срок возврата раньше срока выдачи, то выходим из процедуры с сообщением об ошибке
SET @DateDiff = DATEDIFF(d, @DateBegin, @DateEnd)
IF @DateDiff = 0
	SET @DateDiff = 1
ELSE IF @DateDiff < 0
BEGIN
	SET @ErrorMessage = 'Ошибка! Срок возврата раньше срока выдачи!'
	RAISERROR (@ErrorMessage, 10, 3)
	RETURN
END
--Заносим информацию об аренде в таблицу Rentals
INSERT Rentals
(ClientID, DateBegin, DateEnd)
VALUES
(@ClientID, @DateBegin, @DateEnd)

SET @RentalID = @@IDENTITY
--Заносим до 5 предметов инвентаря в смежную таблицу
EXEC spAddRentEquip @RentalID, @Equip1ID
EXEC spAddRentEquip @RentalID, @Equip2ID
EXEC spAddRentEquip @RentalID, @Equip3ID
EXEC spAddRentEquip @RentalID, @Equip4ID
EXEC spAddRentEquip @RentalID, @Equip5ID
--Вычисляем сумму к оплате
SET @Total = (SELECT SUM(e.Price) * @DateDiff
					FROM Rentals r JOIN RentEquip re ON r.ID = re.RentalID 
					JOIN Equipments e ON re.EquipID = e.ID
					WHERE r.ID = @RentalID)
--Заносим сумму к оплате данной заявки
UPDATE Rentals SET Total = @Total WHERE ID = @RentalID
--Откат всех действий и выход если платеж превышает сумму к оплате
IF @Total < @Payment
BEGIN
	SET @ErrorMessage = 'Платеж превышает сумму к оплате ' 
		+ CAST (@Total AS nvarchar(12)) + ', скорректируйте заявку'
	DELETE FROM RentEquip WHERE RentalID = @RentalID
	DELETE FROM Rentals WHERE ID = @RentalID
	RAISERROR (@ErrorMessage, 10, 3)
	RETURN
END
--Откат всех действий если платеж меньше арендной платы за 1 день
IF @Payment < @Total/@DateDiff
BEGIN
	SET @ErrorMessage = 'Платеж недостаточен. Минимальная сумма к оплате ' 
		+ CAST (@Total/@DateDiff AS nvarchar(12)) + ', скорректируйте заявку'
	DELETE FROM RentEquip WHERE RentalID = @RentalID
	DELETE FROM Rentals WHERE ID = @RentalID
	RAISERROR (@ErrorMessage, 10, 3)
	RETURN
END
--Производим платеж включенный в заявку
EXEC spAddPayment @RentalID, @Payment, @DateBegin, @ErrorMessage OUTPUT
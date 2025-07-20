/****************************************************************************
************************   T R A N S A C T - S Q L   ************************
************************   C O U R S E     W O R K   ************************
*****************************************************************************
*****  Practic I  *******      DATABASE CREATION      ***********************
****************************************************************************/

--1. Создаем базу данных интернет пункта проката инвентаря EquipRentalPoint.
CREATE DATABASE EquipRentalPointDB
	COLLATE Cyrillic_General_CI_AS
GO

--2. Создаем в базе данных таблицы согласно рис.
USE EquipRentalPointDB
GO

--1) Categories (ID,Title).
CREATE TABLE Categories(
	ID int IDENTITY(1,1) NOT NULL,
	Title nvarchar(50) NOT NULL
)
GO

--2) Eguipment(ID, Title, Price).
CREATE TABLE Equipments(
	ID int IDENTITY(1,1) NOT NULL,
	Title nvarchar(50) NOT NULL,
	Price money NOT NULL
)
GO

--3) EquipCategory (EquipID, CategoryID).
CREATE TABLE EquipCategory(
	EquipID int NOT NULL,
	CategoryID int NOT NULL
)
GO

--4) Clients (ID, FullNAme, Phone).
CREATE TABLE Clients(
	ID int IDENTITY(1,1) NOT NULL,
	FullName nvarchar(100) NOT NULL,
	Phone char(11) NOT NULL
)
GO

--5) Rentals (ID, ClientID, DateBegin, DateEnd, Payed, TotalPrice).
CREATE TABLE Rentals(
	ID int IDENTITY(1,1) NOT NULL,
	ClientID int NOT NULL,
	DateBegin date NOT NULL,
	DateEnd date NOT NULL,
	Payed money NOT NULL DEFAULT 0,
	Total money NOT NULL DEFAULT 0,
	is_paid tinyint NOT NULL DEFAULT 0
)
GO

--6) RentEquip (RentalID, EquipID).
CREATE TABLE RentEquip(
	RentalID int NOT NULL,
	EquipID int NOT NULL
)
GO

--7) Payments (ID, RentalID, PaymentDate, Amount).
CREATE TABLE Payments(
	ID int IDENTITY(1,1) NOT NULL,
	RentalID int NOT NULL,
	PaymentDate date NOT NULL,
	Amount money NOT NULL
)
GO


--3. Установливаем связи между таблицами согласно рис.1, необходимо предусмотреть условия ссылочной целостности.
--1) Categories: ID(PK).
ALTER TABLE Categories
	ADD CONSTRAINT PK_Categories PRIMARY KEY(ID)
GO

--2) Eguipments: ID(PK).
ALTER TABLE Equipments
	ADD CONSTRAINT PK_Equipments PRIMARY KEY(ID)
GO

--3) EquipCategory: EquipID,CategoryID(PK), EquipID(FK), CategoryID(FK).
ALTER TABLE EquipCategory
	ADD CONSTRAINT PK_EquipCategory PRIMARY KEY(EquipID, CategoryID)
GO

ALTER TABLE EquipCategory
	ADD CONSTRAINT FK_EquipCategory_Equipments FOREIGN KEY(EquipID)
	REFERENCES Equipments(ID)
	ON DELETE NO ACTION
GO

ALTER TABLE EquipCategory
	ADD CONSTRAINT FK_EquipCategory_Categories FOREIGN KEY(CategoryID)
	REFERENCES Categories(ID)
	ON DELETE NO ACTION
GO

--4) Clients: ID(PK).
ALTER TABLE Clients
	ADD CONSTRAINT PK_Clients PRIMARY KEY(ID)
GO

--5) Rentals: ID(PK), ClientID(FK).
ALTER TABLE Rentals
	ADD CONSTRAINT PK_Rentals PRIMARY KEY(ID)
GO

ALTER TABLE Rentals
	ADD CONSTRAINT FK_Rentals_Clients FOREIGN KEY(ClientID)
	REFERENCES Clients(ID)
	ON DELETE NO ACTION
GO

--6) RentEquip: RentalID,EquipID(PK), RentalID(FK), EquipID(FK).
ALTER TABLE RentEquip
	ADD CONSTRAINT PK_RentEquip PRIMARY KEY(RentalID, EquipID)
GO

ALTER TABLE  RentEquip
	ADD CONSTRAINT FK_RentEquip_Rentals FOREIGN KEY(RentalID)
	REFERENCES Rentals(ID)
	ON DELETE NO ACTION
GO

ALTER TABLE  RentEquip
	ADD CONSTRAINT FK_RentEquip_Equipments FOREIGN KEY(EquipID)
	REFERENCES Equipments(ID)
	ON DELETE NO ACTION
GO

--7) Payments: ID(PK), Rental(FK).
ALTER TABLE Payments
	ADD CONSTRAINT PK_Payments PRIMARY KEY (ID)
GO

ALTER TABLE  Payments
	ADD CONSTRAINT FK_Payments_Rentals FOREIGN KEY(RentalID)
	REFERENCES Rentals(ID)
	ON DELETE NO ACTION
GO

--4. Создаем пользовательские ограничения.

--1) Создаем ограничение на корректность ввода номера телефона.
ALTER TABLE Clients
	ADD CONSTRAINT CH_Clients_Phone
	CHECK (Phone LIKE '8[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
GO

--2) Создаем ограничение уникальности ввода номера клиента.
ALTER TABLE Clients
	ADD CONSTRAINT CH_Clients_PhoneUnique
	UNIQUE (Phone)
GO

--3) Создаем ограничение на положительную цену инвентаря.
ALTER TABLE Equipments
	ADD CONSTRAINT CH_Equipments_Price
	CHECK (Price > 0)
GO

--4) Создаем ограничание на уникальность названия категории.
ALTER TABLE Categories
	ADD CONSTRAINT CH_Categories_TitleUnique
	UNIQUE (Title)
GO

--5) Создаем ограничение на положительный платеж.
ALTER TABLE Payments
	ADD CONSTRAINT CH_Payments_Amount
	CHECK (Amount > 0)
GO

--6) Создаем ограничение на положительную сумму к оплате.
ALTER TABLE Rentals
	ADD CONSTRAINT CH_Rentals_Total
	CHECK (Total >= 0)
GO

--7) Создаем ограничение на положительную внесенную сумму.
ALTER TABLE Rentals
	ADD CONSTRAINT CH_Rentals_Payed
	CHECK (Payed >= 0)
GO

--8) Создаем ограничение на дату выдачи не позже даты возврата.
ALTER TABLE Rentals
	ADD CONSTRAINT CH_Rentals_DateBegin_DateEnd
	CHECK (DateBegin <= DateEnd)
GO

--9) Создаем ограничение на то что оплаты не может быть выше начисленной суммы.
ALTER TABLE Rentals
	ADD CONSTRAINT CH_Rentals_Total_Payed
	CHECK (Payed <=Total)
GO

--5. Создаем триггеры
--1) Триггер на получение флажка is_paid когда заявка полностью оплачена
CREATE TRIGGER tr_Rentals_update_is_paid 
ON Rentals 
AFTER UPDATE 
AS 
BEGIN 
  IF UPDATE(Payed) 
  BEGIN 
    DECLARE @Payed money 
    DECLARE @Total money 
    DECLARE @ID int 

    SELECT @Payed = Payed, @Total = Total, @ID = ID 
    FROM inserted 

    IF @Payed = @Total 
    BEGIN 
      UPDATE Rentals 
      SET is_paid = 1 
      WHERE ID = @ID 
    END 
  END 
END 

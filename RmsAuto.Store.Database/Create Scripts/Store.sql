IF OBJECT_ID('dbo.ShoppingCartItems', 'U') IS NOT NULL DROP TABLE dbo.ShoppingCartItems
GO
IF OBJECT_ID('dbo.OrderLineStatusChanges', 'U') IS NOT NULL DROP TABLE dbo.OrderLineStatusChanges
GO
IF OBJECT_ID('dbo.OrderLines', 'U') IS NOT NULL DROP TABLE dbo.OrderLines
GO
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL DROP TABLE dbo.Orders
GO
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users
GO
IF OBJECT_ID('dbo.UserMaintEntries', 'U') IS NOT NULL DROP TABLE dbo.UserMaintEntries
GO 
IF OBJECT_ID('dbo.UserProfileEntries', 'U') IS NOT NULL DROP TABLE dbo.UserProfileEntries
GO 
IF OBJECT_ID('dbo.SpareParts', 'U') IS NOT NULL DROP TABLE dbo.SpareParts
GO
IF OBJECT_ID('dbo.PricingMatrixEntries', 'U') IS NOT NULL DROP TABLE dbo.PricingMatrixEntries
GO
IF OBJECT_ID('dbo.SupplierCurrencyRates', 'U') IS NOT NULL DROP TABLE dbo.SupplierCurrencyRates
GO
IF OBJECT_ID('dbo.Manufacturers', 'U') IS NOT NULL DROP TABLE dbo.Manufacturers
GO
IF OBJECT_ID('dbo.HandyClientSetEntries', 'U') IS NOT NULL DROP TABLE dbo.HandyClientSetEntries
GO
IF OBJECT_ID('dbo.TransferChangesErrors', 'U') IS NOT NULL DROP TABLE dbo.TransferChangesErrors
GO
IF OBJECT_ID('dbo.TransferChangesLogEntries', 'U') IS NOT NULL DROP TABLE dbo.TransferChangesLogEntries
GO
IF OBJECT_ID('[Vin].[VinRequestItems]', 'U') IS NOT NULL DROP TABLE [Vin].[VinRequestItems]
GO
IF OBJECT_ID('[Vin].[VinRequests]', 'U') IS NOT NULL DROP TABLE [Vin].[VinRequests]
GO
IF OBJECT_ID('[Vin].[UserGarageCars]', 'U') IS NOT NULL DROP TABLE [Vin].[UserGarageCars]
GO
IF OBJECT_ID('dbo.SparePartWithCustomFactors', 'V') IS NOT NULL DROP VIEW dbo.SparePartWithCustomFactors
GO

IF NOT EXISTS (SELECT * FROM sys.schemas WHERE [name] = 'Vin') 
	EXECUTE ('CREATE SCHEMA Vin AUTHORIZATION dbo')


CREATE TABLE dbo.Users
(
	UserID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
	Username varchar(20) NOT NULL,
	Password varchar(50) NOT NULL,
	UserRole tinyint NOT NULL,
	AcctgID varchar(50) NOT NULL,
	CreationTime datetime NOT NULL DEFAULT (getdate()),
	CONSTRAINT UC_Users_Username UNIQUE(Username),
	CONSTRAINT UC_users_AcctgID_Role UNIQUE(AcctgID, UserRole)
)

CREATE TABLE dbo.UserMaintEntries
(
	EntryUid UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_UserMaintEntries PRIMARY KEY,
	EntryPurpose TINYINT NOT NULL,
	Username VARCHAR(20) NULL,
	ClientID VARCHAR(50) NULL,
	RegDataBytes VARBINARY(4000) NULL,
	EntryTime DATETIME NOT NULL DEFAULT(GETDATE())
)
GO

CREATE TABLE dbo.UserProfileEntries
(
	UserID INT NOT NULL 
		CONSTRAINT PK_UserProfileEntries PRIMARY KEY
		CONSTRAINT FK_UserProfileEntries_Users FOREIGN KEY REFERENCES dbo.Users (UserID),
	ProfileEntryBytes VARBINARY(4000) NOT NULL,
	LastActivityTime DATETIME NOT NULL,
	LastUpdateTime DATETIME NOT NULL		
)
GO

CREATE TABLE dbo.SpareParts
(
	Manufacturer VARCHAR(50) NOT NULL,
	PartNumber VARCHAR(50) NOT NULL,
	InternalPartNumber VARCHAR(50) NULL,	
	PartName VARCHAR(255) NOT NULL,
	PartDescription VARCHAR(255) NULL,
	SupplierID INT NOT NULL,
	DeliveryDaysMin INT NOT NULL,
	DeliveryDaysMax INT NOT NULL,
	InitialPrice DECIMAL(18,2) NOT NULL,
	RgCode CHAR(10) NULL,
	WeightPhysical DECIMAL(18,3) NULL,
	WeightVolume DECIMAL(18,3) NULL,
	QtyInStock INT NULL,
	MinOrderQty INT NULL,
	PriceConstantTerm DECIMAL(18,2) NULL
	CONSTRAINT PK_SpareParts PRIMARY KEY CLUSTERED (PartNumber, Manufacturer, SupplierID)			
)
GO

CREATE TABLE dbo.PricingMatrixEntries(
	PricingMatrixEntryID INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PricingMatrixEntries PRIMARY KEY NONCLUSTERED,
	SupplierID INT NOT NULL,
	Manufacturer VARCHAR(50) NULL,
	PartNumber VARCHAR(50) NULL,
	RgCodeSpec CHAR(10) NULL,
	RgCode AS (case when PartNumber IS NULL then RgCodeSpec  end) PERSISTED,
	CorrectionFactor DECIMAL(18, 4) NOT NULL,
	CorrectionFactor39 DECIMAL(18, 4) NOT NULL,
	CustomFactor1 DECIMAL(18, 4) NOT NULL,
	CustomFactor2 DECIMAL(18, 4) NOT NULL,
	CustomFactor3 DECIMAL(18, 4) NOT NULL,
	CustomFactor4 DECIMAL(18, 4) NOT NULL,
	CustomFactor5 DECIMAL(18, 4) NOT NULL,
	CustomFactor6 DECIMAL(18, 4) NOT NULL,
	CustomFactor7 DECIMAL(18, 4) NOT NULL,
	CustomFactor8 DECIMAL(18, 4) NOT NULL,
	CustomFactor9 DECIMAL(18, 4) NOT NULL,
	CustomFactor10 DECIMAL(18, 4) NOT NULL,
	CustomFactor11 DECIMAL(18, 4) NOT NULL,
	CustomFactor12 DECIMAL(18, 4) NOT NULL,
	CustomFactor13 DECIMAL(18, 4) NOT NULL,
	CustomFactor14 DECIMAL(18, 4) NOT NULL,
	CustomFactor15 DECIMAL(18, 4) NOT NULL
	CONSTRAINT UC_PricingMatrixEntries UNIQUE CLUSTERED (SupplierID, Manufacturer, PartNumber, RgCode)
)
GO

CREATE TABLE dbo.ShoppingCartItems
(
	ItemID INT NOT NULL IDENTITY(1,1)
		CONSTRAINT PK_ShoppingCartItems PRIMARY KEY NONCLUSTERED,
	OwnerID INT NOT NULL 
		CONSTRAINT FK_ShoppingCartItems_Users FOREIGN KEY
		REFERENCES dbo.Users (UserID) ON DELETE CASCADE,
	ClientID VARCHAR(50) NOT NULL,
	Manufacturer VARCHAR(50) NOT NULL,
	PartNumber VARCHAR(50) NOT NULL,
	SupplierID INT NOT NULL,
	DeliveryDaysMin INT NOT NULL,
	DeliveryDaysMax INT NOT NULL,
	PartName VARCHAR(100) NOT NULL,
	PartDescription VARCHAR(100) NULL,
	UnitPrice DECIMAL(18,2) NOT NULL,
	Qty INT NOT NULL,
	VinCheckupDataID INT NULL,
	StrictlyThisNumber BIT NOT NULL DEFAULT(0),
	ItemNotes VARCHAR(500) NULL,
	ItemVersion TIMESTAMP NOT NULL
	CONSTRAINT UC_ShoppingCartItems
		UNIQUE CLUSTERED 
		(OwnerID, ClientID, PartNumber, Manufacturer, SupplierID)
)
GO

--identity seed set to 121000 due to possible actual client
--orders in Hansa test base with OrderID < 121000
CREATE TABLE dbo.Orders
(
	OrderID INT NOT NULL IDENTITY(1,121000) CONSTRAINT PK_Orders PRIMARY KEY,
	UserID INT NOT NULL 
		CONSTRAINT FK_Orders_Users FOREIGN KEY
		REFERENCES dbo.Users (UserID),
	ClientID VARCHAR(50) NOT NULL,
	StoreNumber VARCHAR(50) NOT NULL,
	ShippingMethod TINYINT NOT NULL,
	PaymentMethod TINYINT NOT NULL,
	OrderNotes VARCHAR(500) NULL,
	OrderDate DATETIME NOT NULL DEFAULT GETDATE()
)
GO

--identity seed set to 426000 due to possible actual client
--order lines in Hansa test base with OrderLineID < 430000
CREATE TABLE dbo.OrderLines
(
	OrderLineID INT NOT NULL IDENTITY(1,430000) CONSTRAINT PK_OrderLines PRIMARY KEY,
	AcctgOrderLineID INT NOT NULL,
	ParentOrderLineID INT NULL,
	OrderID INT NOT NULL 
		CONSTRAINT FK_OrderLines_Orders FOREIGN KEY
		REFERENCES dbo.Orders (OrderID),
	PartNumber VARCHAR(50) NOT NULL,
	Manufacturer VARCHAR(50) NOT NULL,
	SupplierID INT NOT NULL,
	DeliveryDaysMin INT NOT NULL,
	DeliveryDaysMax INT NOT NULL,
	PartName VARCHAR(100) NOT NULL,
	PartDescription VARCHAR(100) NULL,
	WeightPhysical DECIMAL(18,3) NULL,
	WeightVolume DECIMAL(18,3) NULL,
	UnitPrice DECIMAL(18,2) NOT NULL,
	Qty INT NOT NULL,
	StrictlyThisNumber BIT NOT NULL,
	VinCheckupData VARCHAR(500) NULL,
	OrderLineNotes VARCHAR(500) NULL,
	EstSupplyDate DATETIME NULL,
	CurrentStatus TINYINT NOT NULL,
	CurrentStatusDate DATETIME NULL
	CONSTRAINT UC_OrderLines_AcctgOrderLineID UNIQUE (AcctgOrderLineID)
)
go

CREATE TABLE dbo.OrderLineStatusChanges
(
	OrderLineStatusChangeID INT NOT NULL IDENTITY(1,1) 
		CONSTRAINT PK_OrderLineStatusChanges PRIMARY KEY,
	OrderLineID INT NOT NULL 
		CONSTRAINT FK_OrderLineStatusChanges_OrderLines FOREIGN KEY
		REFERENCES dbo.OrderLines (OrderLineID),
	OrderLineStatus TINYINT NOT NULL,
	StatusChangeInfo VARCHAR(100) NULL,
	StatusChangeTime DATETIME NOT NULL,
	ClientReaction TINYINT NULL,
	ClientReactionTime DATETIME NULL
	CONSTRAINT UC1_OrderLineStatusChanges 
		UNIQUE (OrderLineID, StatusChangeTime, OrderLineStatus)			
)
GO


CREATE TABLE dbo.TransferChangesLogEntries
(
	LogEntryID INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_TransferChangesLogEntries PRIMARY KEY,
	CheckpointTime DATETIME NOT NULL,
	MinChangeTime DATETIME NULL,
	MaxChangeTime DATETIME NULL,
	ChangesReceived INT NOT NULL,
	OrderLinesAdded INT NOT NULL,
	StatusChangesAdded INT NOT NULL,
	TransferStartTime DATETIME NOT NULL,
	DurationInMilliseconds INT NOT NULL
)
GO

CREATE TABLE dbo.TransferChangesErrors
(
	ChangesErrorID INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_TransferChangesErrors PRIMARY KEY,
	LogEntryID INT NOT NULL 
		CONSTRAINT FK_TransferChangesErrors_TransferChangesLogEntries FOREIGN KEY
		REFERENCES dbo.TransferChangesLogEntries (LogEntryID) ON DELETE CASCADE,
	ErrorMessage VARCHAR(500) NOT NULL	
)
GO

CREATE TABLE dbo.HandyClientSetEntries
(
	ManagerID INT NOT NULL
		CONSTRAINT FK_HandyClientSetEntries_Users FOREIGN KEY
		REFERENCES dbo.Users (UserID) ON DELETE CASCADE,
	ClientID VARCHAR(50) NOT NULL,
	IsDefault BIT NOT NULL DEFAULT(0)
	CONSTRAINT PK_HandyClientSetEntries
		PRIMARY KEY CLUSTERED (ManagerID, ClientID)
)
GO



CREATE TABLE [Vin].[UserGarageCars](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [varchar](17) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Brand] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Model] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Modification] [nvarchar](100) COLLATE Cyrillic_General_CI_AS NULL,
	[EngineNumber] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[EngineType] [tinyint] NOT NULL CONSTRAINT [DF_UserGarageCars_EngineType]  DEFAULT ((0)),
	[EngineCCM] [smallint] NULL,
	[EngineHP] [smallint] NULL,
	[BodyType] [tinyint] NOT NULL CONSTRAINT [DF_UserGarageCars_BodyType]  DEFAULT ((0)),
	[DoorsNumber] [tinyint] NULL,
	[Year] [smallint] NOT NULL,
	[Month] [tinyint] NULL,
	[VIN] [varchar](17) COLLATE Cyrillic_General_CI_AS NULL,
	[Frame] [varchar](17) COLLATE Cyrillic_General_CI_AS NULL,
	[HasABS] [bit] NULL,
	[HasConditioner] [bit] NULL,
	[TransmissionType] [tinyint] NOT NULL CONSTRAINT [DF_UserGarageCars_TransmissionType]  DEFAULT ((0)),
	[TransmissionNumber] [varchar](17) COLLATE Cyrillic_General_CI_AS NULL,
	[DriveType] [tinyint] NOT NULL CONSTRAINT [DF_UserGarageCars_DriveType]  DEFAULT ((0)),
	[AddedDate] [datetime] NOT NULL CONSTRAINT [DF_UserGarageCars_AddedDate]  DEFAULT (getdate()),
	 CONSTRAINT [PK_VINUserGarageCars] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) 

GO
CREATE TABLE [Vin].[VinRequests](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [varchar](17) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[RequestDate] [datetime] NOT NULL CONSTRAINT [DF_VINRequests_RequestDate]  DEFAULT (getdate()),
	[AnswerDate] [datetime] NULL,
	[Proceeded] [bit] NOT NULL CONSTRAINT [DF_VINRequests_State]  DEFAULT ((0)),
	[HasNotBeenSeen] [bit] NOT NULL CONSTRAINT [DF_VinRequests_HasNotBeenSeen]  DEFAULT ((0)),
	[Brand] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[Model] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[Modification] [nvarchar](100) COLLATE Cyrillic_General_CI_AS NULL,
	[EngineNumber] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[EngineType] [tinyint] NOT NULL CONSTRAINT [DF_VinRequests_EngineType]  DEFAULT ((0)),
	[EngineCCM] [smallint] NULL,
	[EngineHP] [smallint] NULL,
	[BodyType] [tinyint] NOT NULL CONSTRAINT [DF_VinRequests_BodyType]  DEFAULT ((0)),
	[DoorsNumber] [tinyint] NULL,
	[Year] [smallint] NOT NULL,
	[Month] [tinyint] NULL,
	[VIN] [varchar](17) COLLATE Cyrillic_General_CI_AS NULL,
	[Frame] [varchar](17) COLLATE Cyrillic_General_CI_AS NULL,
	[HasABS] [bit] NULL,
	[HasConditioner] [bit] NULL,
	[TransmissionType] [tinyint] NOT NULL CONSTRAINT [DF_VinRequests_TransmissionType]  DEFAULT ((0)),
	[TransmissionNumber] [varchar](17) COLLATE Cyrillic_General_CI_AS NULL,
	[DriveType] [tinyint] NULL,
	[StoreId] [varchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[ManagerId] [varchar](50) COLLATE Cyrillic_General_CI_AS NULL,
 CONSTRAINT [PK_VINRequest] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [Vin].[VinRequestItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RequestId] [int] NULL,
	[Name] [nvarchar](100) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Quantity] [smallint] NOT NULL,
	[Description] [nvarchar](300) COLLATE Cyrillic_General_CI_AS NULL,
	[PartNumber] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[Manufacturer] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[DeliveryDays] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[PricePerUnit] [decimal](18, 0) NULL,
	[PartNumberOriginal] [nvarchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[ManagerComment] [nvarchar](max) COLLATE Cyrillic_General_CI_AS NULL,
 CONSTRAINT [PK_Vin.VinRequestItemsAlt] PRIMARY KEY NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Vin].[VinRequestItems]  WITH CHECK ADD  CONSTRAINT [FK_VinRequestItemsAlt_VinRequests] FOREIGN KEY([RequestId])
REFERENCES [Vin].[VinRequests] ([Id])
GO
ALTER TABLE [Vin].[VinRequestItems] CHECK CONSTRAINT [FK_VinRequestItemsAlt_VinRequests]
GO





--- INDEXES ---
CREATE INDEX IX_SpareParts_SupplierID_Manufacturer ON SpareParts (SupplierID,Manufacturer)
Go
-----------------------

CREATE TABLE [SparePartCrosses](
	[PartNumberOriginal] [varchar](50) not null,
	[ManufacturerOriginal] [varchar](50) not null,
	[PartNumber] [varchar](50) not null,
	[Manufacturer] [varchar](50) not null,
	[CrossType] [tinyint] not null
)
GO
--(UNIQUE CLUSTERED INDEX WITH IGNORE_DUP_KEY INSTEAD OF PRIMARY KEY IS ESSENTIAL!)
CREATE UNIQUE CLUSTERED INDEX [UC_SparePartCrosses] ON [dbo].[SparePartCrosses] 
(
	[PartNumberOriginal] ASC,
	[ManufacturerOriginal] ASC,
	[PartNumber] ASC,
	[Manufacturer] ASC
)
WITH (IGNORE_DUP_KEY = ON)
GO
---2009-12-22---

alter table dbo.Orders add CustOrderNum varchar(50)

---2010-01-14---
GO

create view SparePartWithCustomFactors as
select p.Manufacturer,p.PartNumber,p.InternalPartNumber,p.PartName,p.PartDescription,p.SupplierID,
      p.DeliveryDaysMin,p.DeliveryDaysMax,p.InitialPrice,p.RgCode,p.WeightPhysical,p.WeightVolume,
      p.QtyInStock,p.MinOrderQty,p.PriceConstantTerm,
	e.PricingMatrixEntryID,e.CorrectionFactor,e.CorrectionFactor39,
	e.CustomFactor1,e.CustomFactor2,e.CustomFactor3,e.CustomFactor4,e.CustomFactor5,
	e.CustomFactor6,e.CustomFactor7,e.CustomFactor8,e.CustomFactor9,e.CustomFactor10,
	e.CustomFactor11,e.CustomFactor12,e.CustomFactor13,e.CustomFactor14,e.CustomFactor15	
from SpareParts as p 
join PricingMatrixEntries e on e.PricingMatrixEntryID = 
	(select top 1 PricingMatrixEntryID from 
		(select PricingMatrixEntryID from PricingMatrixEntries e1 where e1.SupplierID=p.SupplierID and e1.Manufacturer=p.Manufacturer and e1.PartNumber=p.PartNumber and e1.RgCode is null
		union all select PricingMatrixEntryID from PricingMatrixEntries e2 where e2.SupplierID=p.SupplierID and e2.Manufacturer=p.Manufacturer and e2.PartNumber is null and e2.RgCode=p.RgCode 
		union all select PricingMatrixEntryID from PricingMatrixEntries e3 where e3.SupplierID=p.SupplierID and e3.Manufacturer=p.Manufacturer and e3.PartNumber is null and  e3.RgCode is null
		union all select PricingMatrixEntryID from PricingMatrixEntries e4 where e4.SupplierID=p.SupplierID and e4.Manufacturer is null and e4.PartNumber is null and e4.RgCode is null) T)

GO

---2010-01-21---

alter table SpareParts add PriceDate datetime not null default getdate()
go

alter view SparePartWithCustomFactors as
select p.Manufacturer,p.PartNumber,p.InternalPartNumber,p.PartName,p.PartDescription,p.SupplierID,
      p.DeliveryDaysMin,p.DeliveryDaysMax,p.InitialPrice,p.RgCode,p.WeightPhysical,p.WeightVolume,
      p.QtyInStock,p.MinOrderQty,p.PriceConstantTerm,p.PriceDate,
	e.PricingMatrixEntryID,e.CorrectionFactor,e.CorrectionFactor39,
	e.CustomFactor1,e.CustomFactor2,e.CustomFactor3,e.CustomFactor4,e.CustomFactor5,
	e.CustomFactor6,e.CustomFactor7,e.CustomFactor8,e.CustomFactor9,e.CustomFactor10,
	e.CustomFactor11,e.CustomFactor12,e.CustomFactor13,e.CustomFactor14,e.CustomFactor15	
from SpareParts as p 
join PricingMatrixEntries e on e.PricingMatrixEntryID = 
	(select top 1 PricingMatrixEntryID from 
		(select PricingMatrixEntryID from PricingMatrixEntries e1 where e1.SupplierID=p.SupplierID and e1.Manufacturer=p.Manufacturer and e1.PartNumber=p.PartNumber and e1.RgCode is null
		union all select PricingMatrixEntryID from PricingMatrixEntries e2 where e2.SupplierID=p.SupplierID and e2.Manufacturer=p.Manufacturer and e2.PartNumber is null and e2.RgCode=p.RgCode 
		union all select PricingMatrixEntryID from PricingMatrixEntries e3 where e3.SupplierID=p.SupplierID and e3.Manufacturer=p.Manufacturer and e3.PartNumber is null and  e3.RgCode is null
		union all select PricingMatrixEntryID from PricingMatrixEntries e4 where e4.SupplierID=p.SupplierID and e4.Manufacturer is null and e4.PartNumber is null and e4.RgCode is null) T)

go
---2010-02-11---


alter table OrderLines alter column [PartName] [varchar](255) NOT NULL
alter table OrderLines alter column [PartDescription] [varchar](255) NULL
alter table ShoppingCartItems alter column [PartName] [varchar](255) NOT NULL
alter table ShoppingCartItems alter column [PartDescription] [varchar](255) NULL
go
---2010-03-09---


IF OBJECT_ID('dbo.SparePartCrossesBrands', 'U') IS NOT NULL DROP TABLE dbo.SparePartCrossesBrands
IF OBJECT_ID('dbo.SparePartCrossesGroups', 'U') IS NOT NULL DROP TABLE dbo.SparePartCrossesGroups
IF OBJECT_ID('dbo.SparePartCrossesLinks', 'U') IS NOT NULL DROP TABLE dbo.SparePartCrossesLinks
GO

CREATE TABLE SparePartCrossesBrands
(
	ManufacturerMain varchar(50) not null,
	Manufacturer varchar(50) not null,
	Flag1 int not null
)

CREATE CLUSTERED INDEX IC_SparePartCrossesBrands ON SparePartCrossesBrands (ManufacturerMain) WITH (FILLFACTOR=75)
CREATE UNIQUE INDEX UX_SparePartCrossesBrands ON SparePartCrossesBrands (Manufacturer) INCLUDE (ManufacturerMain) WITH (IGNORE_DUP_KEY = ON, FILLFACTOR=75)
GO

CREATE TABLE SparePartCrossesGroups
(
	GreatGroupID int not null,
	GroupID int not null,
	Manufacturer varchar(50) not null,
	PartNumber varchar(50) not null,
	Flag1 int not null,
	Flag2 int not null	
)

--CREATE UNIQUE CLUSTERED INDEX UC_SparePartCrossesGroups ON SparePartCrossesGroups (Manufacturer,PartNumber) WITH (IGNORE_DUP_KEY = ON)
--CREATE INDEX IX_SparePartCrossesGroups ON SparePartCrossesGroups (GreatGroupID,GroupID)

CREATE CLUSTERED INDEX IC_SparePartCrossesGroups ON SparePartCrossesGroups (GreatGroupID) WITH (FILLFACTOR=75)
CREATE UNIQUE INDEX UX_SparePartCrossesGroups ON SparePartCrossesGroups (PartNumber,Manufacturer) WITH (IGNORE_DUP_KEY = ON, FILLFACTOR=75)

GO
CREATE TABLE SparePartCrossesLinks
(
	GreatGroupID1 int not null,
	GreatGroupID2 int not null,
	Flag1 int not null
)

CREATE UNIQUE CLUSTERED INDEX UC_SparePartCrossesLinks ON SparePartCrossesLinks (GreatGroupID1,GreatGroupID2) WITH (IGNORE_DUP_KEY = ON, FILLFACTOR=75)

GO

---2010-04-05---

create table ClientAlertInfo
(
	ClientID varchar(50) not null primary key,
	OrderTrackingLastAlertDate datetime not null
)

insert into ClientAlertInfo (ClientID,OrderTrackingLastAlertDate) 
select AcctgId,GETDATE() from Users where UserRole=0

---2010-04-12---

alter table SpareParts add SparePartGroupID int
go

ALTER view [dbo].[SparePartWithCustomFactors] as
select p.Manufacturer,p.PartNumber,p.InternalPartNumber,p.PartName,p.PartDescription,p.SupplierID,
      p.DeliveryDaysMin,p.DeliveryDaysMax,p.InitialPrice,p.RgCode,p.WeightPhysical,p.WeightVolume,
      p.QtyInStock,p.MinOrderQty,p.PriceConstantTerm,p.PriceDate,p.SparePartGroupID,
	e.PricingMatrixEntryID,e.CorrectionFactor,e.CorrectionFactor39,
	e.CustomFactor1,e.CustomFactor2,e.CustomFactor3,e.CustomFactor4,e.CustomFactor5,
	e.CustomFactor6,e.CustomFactor7,e.CustomFactor8,e.CustomFactor9,e.CustomFactor10,
	e.CustomFactor11,e.CustomFactor12,e.CustomFactor13,e.CustomFactor14,e.CustomFactor15	
from SpareParts as p 
join PricingMatrixEntries e on e.PricingMatrixEntryID = 
	(select top 1 PricingMatrixEntryID from 
		(select PricingMatrixEntryID from PricingMatrixEntries e1 where e1.SupplierID=p.SupplierID and e1.Manufacturer=p.Manufacturer and e1.PartNumber=p.PartNumber and e1.RgCode is null
		union all select PricingMatrixEntryID from PricingMatrixEntries e2 where e2.SupplierID=p.SupplierID and e2.Manufacturer=p.Manufacturer and e2.PartNumber is null and e2.RgCode=p.RgCode 
		union all select PricingMatrixEntryID from PricingMatrixEntries e3 where e3.SupplierID=p.SupplierID and e3.Manufacturer=p.Manufacturer and e3.PartNumber is null and  e3.RgCode is null
		union all select PricingMatrixEntryID from PricingMatrixEntries e4 where e4.SupplierID=p.SupplierID and e4.Manufacturer is null and e4.PartNumber is null and e4.RgCode is null) T)
GO

---2010-04-21---

delete from ClientAlertInfo 

insert into ClientAlertInfo (ClientID,OrderTrackingLastAlertDate) 
select AcctgId,GETDATE() from Users where UserRole=0
GO

---2010-04-26---

alter table ShoppingCartItems add [ReferenceID] [varchar](50) NULL
GO
create INDEX IX_ShoppingCartItems_ReferenceID ON ShoppingCartItems(ReferenceID)
GO

alter table OrderLines add [ReferenceID] [varchar](50) NULL
GO
create INDEX IX_OrderLines_ReferenceID ON OrderLines (ReferenceID)
GO
----2010-06-25---

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Cms].[OrderLineStatuses](
	[OrderLineStatusID] [tinyint] IDENTITY(10,10) NOT FOR REPLICATION NOT NULL ,
	[NameRMS] [varchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[NameHansa] [varchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[IsFinal] BIT NOT NULL DEFAULT(0),
	[RequiresClientReaction] BIT NOT NULL DEFAULT(0),
	[ExcludeFromTotalSum] BIT NOT NULL DEFAULT(0),
	[Manager] [varchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	[Client] [varchar](50) COLLATE Cyrillic_General_CI_AS NOT NULL,
	
 CONSTRAINT [PK_OrderLineStatuses] PRIMARY KEY CLUSTERED 
(
	[OrderLineStatusID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

/* INSERT STRINGS */

SET IDENTITY_INSERT [Cms].[OrderLineStatuses] ON
GO

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('10', 'New', 'NEW', 'новый заказ', 'новый заказ');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('20', 'Processing', '**processing**', 'обработка', 'обработка');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('30', 'WaitingForPayment', 'PREPAID', 'приостановлен', 'приостановлен');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('40', 'Received', 'APPLY', 'заказ принят', 'заказ принят');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('50', 'OrderedFromSupplier', 'SENDSRC', 'отправлен поставщику', 'отправлен поставщику');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('60', 'ShippedBySupplier', 'SEND', '**отгружен поставщиком**', '**отгружен поставщиком**');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('70', 'QuantityAdjustment', 'QTYCHNG', 'изменение количества', 'изменение количества');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, RequiresClientReaction, Manager, Client) VALUES ('80', 'ShipmentDelay', 'DELAY', '1', 'задержка', 'задержка');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, RequiresClientReaction, Manager, Client) VALUES ('90', 'PartNumberTransition', 'NUMCHNG', '1', 'переход номера', 'переход номера');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, RequiresClientReaction, Manager, Client) VALUES ('100', 'PriceAdjustment', 'PRICECHNG', '1', 'изменение цены', 'изменение цены');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('110', 'Approved', 'APPLYCLT', '**подтвержден клиентом**', '**подтвержден клиентом**');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('120', 'InStock', 'ONSTORE', 'поступил на склад', 'поступил на склад');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('130', 'NotFoundInStock', '**not found**', 'не найден', 'не найден');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('140', 'ReadyForDelivery', 'READY', 'готово к выдаче', 'готово к выдаче');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, Manager, Client) VALUES ('150', 'InTransitToClient', 'DELIVERY', 'доставляется клиенту', 'доставляется клиенту');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, IsFinal, Manager, Client) VALUES ('160', 'ReceivedByClient', 'RECEIVE', '1', 'получен клиентом', 'получен клиентом');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, IsFinal, ExcludeFromTotalSum, Manager, Client) VALUES ('170', 'Discontinued', 'FALSH', '1', '1', 'фальшномер', 'фальшномер');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, IsFinal, ExcludeFromTotalSum, Manager, Client) VALUES ('180', 'RefusedBySupplier', 'IMPOSS', '1', '1', 'поставка невозможна', 'поставка невозможна');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, IsFinal, Manager, Client) VALUES ('190', 'NonDelivery', 'NON', '1', 'выдача невозможна', 'выдача невозможна');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, IsFinal, ExcludeFromTotalSum, Manager, Client) VALUES ('200', 'Cancelled', 'CANCEL', '1', '1', 'отменен клиентом', 'отменен клиентом');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, IsFinal, Manager, Client) VALUES ('210', 'Returned', 'RETURN', '1', '**возвращен клиентом**', '**возвращен клиентом**');

INSERT INTO Cms.OrderLineStatuses (OrderLineStatusID, NameRMS, NameHansa, IsFinal, ExcludeFromTotalSum, Manager, Client) VALUES ('220', 'Rejected', 'CANCELT', '1', '1', 'техническая отмена', 'техническая отмена');

/* ADD Foreign Key to OrderLines */

ALTER TABLE [dbo].[OrderLines]  WITH CHECK ADD  CONSTRAINT [FK_OrderLines_OrderLineStatuses] FOREIGN KEY([CurrentStatus])
REFERENCES [Cms].[OrderLineStatuses] ([OrderLineStatusID]);

/* ADD Foreign Key to OrderLineStatusChange */

ALTER TABLE [dbo].[OrderLineStatusChanges]  WITH CHECK ADD  CONSTRAINT [FK_OrderLineStatusChanges_OrderLineStatuses] FOREIGN KEY([OrderLineStatus])
REFERENCES [Cms].[OrderLineStatuses] ([OrderLineStatusID]);

----2010-07-05---

alter table OrderLines add [Processed] [tinyint] NOT NULL DEFAULT(0)
--0 - не обработано
--1 - обработано клиентом
--2 - обработано менеджером

----2010-07-16---

INSERT INTO Cms.TextItems VALUES ('SpareParts.DeliveryHint', 'SpareParts.DeliveryHint',
'<span style="font-size: xx-small;"><span style="color: #ff0000;">*</span> срок поставки исчисляется в рабочих днях, начиная с даты размещения заказа</span>', 'True');
GO
----2010-07-19---

INSERT INTO Cms.TextItems VALUES ('Banners.FooterPartners', '',
'<span style="font-size: xx-small;"><span style="color: #ff0000;">*</span> Баннер в Footer</span>', 'True');
GO

---- Create Folder for Banners
INSERT INTO [Cms].[Folders]
           ([FolderName], 
           [FolderCreationDate])
     VALUES
           ('Баннеры',
           GETDATE())


CREATE TABLE [Cms].[Banners](
	[BannerID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL ,
	[Name] [varchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[URL] [varchar](256) COLLATE Cyrillic_General_CI_AS NULL,
	[FileID] [int] NULL,
	[Html] [varchar](max) COLLATE Cyrillic_General_CI_AS NULL, 
	[RenderType] [tinyint] NOT NULL DEFAULT(0),
	-- 0 Image + URL
	-- 1 Html
	-- 2 File + Html -- например Flash-ролик в Html обертке
	
 CONSTRAINT [PK_Banners] PRIMARY KEY CLUSTERED 
(
	[BannerID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

/* ADD Foreign Key From Banners to Files for File */
ALTER TABLE [Cms].[Banners]  WITH CHECK ADD  CONSTRAINT [FK_Banners_Files_FileID] FOREIGN KEY([FileID])
REFERENCES [Cms].[Files] ([FileID]) ON DELETE SET NULL;
GO
--- При удалении из таблицы файлов, то в Banners выставится в поле FileID NULL

CREATE INDEX IX_Banners_FileID ON [Cms].[Banners] (FileID)
GO

--- Таблица, связей между Разделами сайта и баннерами (Один баннер во многих разделах).
CREATE TABLE [Cms].[BannersForCatalogItems](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[BannerID] [int] NOT NULL,
	[CatalogItemID] [int] NOT NULL,
	[Position] [tinyint] NOT NULL DEFAULT(0),
	[IsVisible] BIT NOT NULL DEFAULT(0),
 CONSTRAINT [PK_BannersForCatalogItems] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

/* ADD Foreign Key From BannersForCatalogItems to Banners*/
ALTER TABLE [Cms].[BannersForCatalogItems] WITH CHECK ADD CONSTRAINT [FK_BannersForCatalogItems_Banners] FOREIGN KEY([BannerID])
REFERENCES [Cms].[Banners] ([BannerID]);
GO

CREATE INDEX IX_BannersForCatalogItems_BannerID ON [Cms].[BannersForCatalogItems] (BannerID)
GO

/* ADD Foreign Key From BannersForCatalogItems to CatalogItems*/
ALTER TABLE [Cms].[BannersForCatalogItems] WITH CHECK ADD CONSTRAINT [FK_BannersForCatalogItems_CatalogItems] FOREIGN KEY([CatalogItemID])
REFERENCES [Cms].[CatalogItems] ([CatalogItemID]);
GO

CREATE INDEX IX_BannersForCatalogItems_CatalogItemID ON [Cms].[BannersForCatalogItems] (CatalogItemID)
GO

/* ADD Column to CatalogItems*/
ALTER TABLE [Cms].[CatalogItems] add [BannerCount] [tinyint] NOT NULL DEFAULT (1);
GO

--- В Default.aspx применяется шаблон PageTreeColumns (должно называться PageThreeColums), там 2 места под баннеры.
--- Во всех остальных местах применяется PageTwoColums, там по 1 баннеро-месту.
UPDATE [Cms].[CatalogItems]
SET BannerCount = '2'
WHERE CatalogItemCode = '~'
GO
/* ADD CONSTRAINT to [Cms].[BannersForCatalogItems] Position + 1 must be <= BannerCount */

CREATE FUNCTION [Cms].[CheckPosition]
(
    @CatalogItemID int
)
RETURNS int
AS
BEGIN
    DECLARE @RetVal int
    SET @RetVal = (SELECT BannerCount
                     FROM [Cms].[CatalogItems]
                     WHERE CatalogItemID = @CatalogItemID)
    RETURN @RetVal
END
GO

ALTER TABLE [Cms].[BannersForCatalogItems] WITH CHECK ADD CONSTRAINT [CK_Position]CHECK ([Position] < [Cms].[CheckPosition]([CatalogItemID]))
GO

---- Тестирование функции для CONSTRAINT, в примере на вход передается ID корневого раздела каталога
--DECLARE @Result int
--exec @Result = Cms.CheckPosition[115]
--Select @Result AS 'Result';

---ALTER TABLE [Cms].[BannersForCatalogItems] CHECK CONSTRAINT [CK_Position]

----Name, Image, Url, Html, File Visible

----2010-07-20---

--INSERT INTO Cms.TextItems VALUES ('ReorderFinish.Client', '',
--'обработано клиентом', 'True');
--GO

--INSERT INTO Cms.TextItems VALUES ('ReorderFinish.Manager', '',
--'обработано менеджером', 'True');
--GO

----2010-07-21---
CREATE TABLE [Acctg].[RequestRepeatCount](
	[ID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[RequestName] [varchar](50) COLLATE Cyrillic_General_CI_AS NULL,
	[RepeatCount] [int] NOT NULL,
 CONSTRAINT [PK_RequestRepeatCount] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

INSERT INTO Acctg.RequestRepeatCount (RequestName, RepeatCount) VALUES ('GetRef', '3');

INSERT INTO Acctg.RequestRepeatCount (RequestName, RepeatCount) VALUES ('GetRates', '3');

INSERT INTO Acctg.RequestRepeatCount (RequestName, RepeatCount) VALUES ('GetShops', '3');

INSERT INTO Acctg.RequestRepeatCount (RequestName, RepeatCount) VALUES ('GetEmployeesRef', '3');

INSERT INTO Acctg.RequestRepeatCount (RequestName, RepeatCount) VALUES ('GetClient', '3');

INSERT INTO Acctg.RequestRepeatCount (RequestName, RepeatCount) VALUES ('FindClient', '3');

INSERT INTO Acctg.RequestRepeatCount (RequestName, RepeatCount) VALUES ('SetEmail', '3');

INSERT INTO Acctg.RequestRepeatCount (RequestName, RepeatCount) VALUES ('GetSellerInfo', '3');

----2010-08-02---

--BeforeTime (раньше) 
--OnTime (вовремя) 
--Delay (с задержкой) 
--NonDelivery (непоставка) 
ALTER TABLE [dbo].[PricingMatrixEntries] add [BeforeTime] [int] NULL;
GO
ALTER TABLE [dbo].[PricingMatrixEntries] add [OnTime] [int] NULL;
GO
ALTER TABLE [dbo].[PricingMatrixEntries] add [Delay] [int] NULL;
GO
ALTER TABLE [dbo].[PricingMatrixEntries] add [NonDelivery] [int] NULL;
GO

ALTER view [dbo].[SparePartWithCustomFactors] as
select p.Manufacturer,p.PartNumber,p.InternalPartNumber,p.PartName,p.PartDescription,p.SupplierID,
      p.DeliveryDaysMin,p.DeliveryDaysMax,p.InitialPrice,p.RgCode,p.WeightPhysical,p.WeightVolume,
      p.QtyInStock,p.MinOrderQty,p.PriceConstantTerm,p.PriceDate,p.SparePartGroupID,
	e.PricingMatrixEntryID,e.CorrectionFactor,e.CorrectionFactor39,
	e.CustomFactor1,e.CustomFactor2,e.CustomFactor3,e.CustomFactor4,e.CustomFactor5,
	e.CustomFactor6,e.CustomFactor7,e.CustomFactor8,e.CustomFactor9,e.CustomFactor10,
	e.CustomFactor11,e.CustomFactor12,e.CustomFactor13,e.CustomFactor14,e.CustomFactor15,
	e.BeforeTime,e.OnTime,e.Delay,e.NonDelivery
from SpareParts as p 
join PricingMatrixEntries e on e.PricingMatrixEntryID = 
	(select top 1 PricingMatrixEntryID from 
		(select PricingMatrixEntryID from PricingMatrixEntries e1 where e1.SupplierID=p.SupplierID and e1.Manufacturer=p.Manufacturer and e1.PartNumber=p.PartNumber and e1.RgCode is null
		union all select PricingMatrixEntryID from PricingMatrixEntries e2 where e2.SupplierID=p.SupplierID and e2.Manufacturer=p.Manufacturer and e2.PartNumber is null and e2.RgCode=p.RgCode 
		union all select PricingMatrixEntryID from PricingMatrixEntries e3 where e3.SupplierID=p.SupplierID and e3.Manufacturer=p.Manufacturer and e3.PartNumber is null and  e3.RgCode is null
		union all select PricingMatrixEntryID from PricingMatrixEntries e4 where e4.SupplierID=p.SupplierID and e4.Manufacturer is null and e4.PartNumber is null and e4.RgCode is null) T)
GO

----2010-08-06---

INSERT INTO Cms.TextItems VALUES ('FeedbackHint', '',
'<span style="font-size: xx-small;">Уважаемый покупатель!<br/>Данный раздел предназначен для решения Ваших общих вопросов, связанных с сотрудничеством, регистрацией на сайте, вопросов связанных с доставкой заказов, работы оптово-розничных отделов, техподдержки пользователей на сайте и т.д.<br/>По всем остальным вопросам, связанным с заключением договоров, предоставлением прайсов и скидок, доступом на наш ftp, условиям работы с поставщиками и др. требующим вмешательства квалифицированных специалистов из нужной Вам области просьба звонить по телефону +7(495)<span style="color: #ff0000;">585-5-585</span></span>', 'True');
GO

----2010-08-19---
---------------------------------------------------------
-- Все изменения схемы необходимо писать в конце файла --
---------------------------------------------------------
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE [name] = 'Cms') 
	EXECUTE ('CREATE SCHEMA Cms AUTHORIZATION dbo')
GO

IF OBJECT_ID('Cms.Files', 'U') IS NOT NULL DROP TABLE Cms.Files
GO
IF OBJECT_ID('Cms.Folders', 'U') IS NOT NULL DROP TABLE Cms.Folders
GO
create table Cms.Folders
(
	FolderID int identity primary key,
	ParentID int references Cms.Folders (FolderID),
	FolderName varchar(250) not null,
	FolderCreationDate datetime not null
)
create table Cms.Files
(
	FileID int identity primary key,
	FolderID int not null references Cms.Folders (FolderID),
	FileName varchar(250) not null,
	FileBody varbinary(max) not null,
	FileMimeType varchar(50) not null,
	FileSize int not null,
	FileCreationDate datetime not null,
	FileModificationDate datetime not null,
	FileNote varchar(500),
	IsImage bit not null,
	ImageWidth int null,
	ImageHeight int null,
	Timestamp TIMESTAMP
)
GO

IF OBJECT_ID('Cms.NewsItems', 'U') IS NOT NULL DROP TABLE Cms.NewsItems
GO
CREATE TABLE Cms.NewsItems
(
	NewsItemID INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_NewsItems PRIMARY KEY,
	NewsItemDate DATETIME NOT NULL,
	NewsItemHeader VARCHAR(500) NOT NULL,
	NewsItemAnnotation VARCHAR(MAX) NOT NULL,
	NewsItemText VARCHAR(MAX) NOT NULL,
	NewsItemVisible BIT NOT NULL
) 
GO

IF OBJECT_ID('Cms.TextItems', 'U') IS NOT NULL DROP TABLE Cms.TextItems
GO
CREATE TABLE Cms.TextItems
(
	TextItemID INT NOT NULL IDENTITY(1,1) CONSTRAINT PK_TextItems PRIMARY KEY,
	TextItemHeader VARCHAR(500) NOT NULL,
	TextItemBody VARCHAR(MAX) NOT NULL
) 
GO

IF OBJECT_ID('Cms.CatalogItems', 'U') IS NOT NULL DROP TABLE Cms.CatalogItems
GO
CREATE TABLE Cms.CatalogItems
(
	CatalogItemID INT IDENTITY PRIMARY KEY,
	ParentItemID INT REFERENCES Cms.CatalogItems (CatalogItemID),
	CatalogItemPriority INT NOT NULL,
	CatalogItemName VARCHAR(500) NOT NULL,
	CatalogItemCode VARCHAR(50) NOT NULL,
	CatalogItemPath VARCHAR(250) NOT NULL,
	CatalogItemQueryString VARCHAR(250),
	CatalogItemOpenNewWindow BIT NOT NULL,
	CatalogItemVisible BIT NOT NULL,
	
	CatalogItemTitle VARCHAR(500),
	CatalogItemKeywords VARCHAR(MAX),
	CatalogItemDescription VARCHAR(MAX),
	
	CatalogItemMenuType TINYINT NOT NULL,
	CatalogItemImageUrl VARCHAR(50)
)
go

IF OBJECT_ID('Cms.Shops', 'U') IS NOT NULL DROP TABLE Cms.Shops
GO
create table Cms.Shops
(
	ShopID int identity primary key,
	ShopName varchar(500) not null,
	ShopAddress varchar(MAX) not null,
	ShopWorkTime varchar(250) not null,
	ShopPhones varchar(MAX) not null,
	ShopNote varchar(MAX),
	ShopVisible bit not null,
	ShopMapFileID int references Cms.Files (FileID),
	ShopGalleryFolderID int references Cms.Folders (FolderID)
)
go

IF OBJECT_ID('Cms.Employees', 'U') IS NOT NULL DROP TABLE Cms.Employees
GO
create table Cms.Employees
(
	EmpID int identity primary key,
	ShopID int not null references Cms.Shops(ShopID),
	EmpName varchar(500) not null,
	EmpPosition varchar(500) not null,
	EmpEmail varchar(250),
	EmpICQ varchar(100),
	EmpPhone varchar(100),
	EmpVisible bit not null
)
GO
IF OBJECT_ID('Cms.Vacancies', 'U') IS NOT NULL DROP TABLE Cms.Vacancies
go
create table Cms.Vacancies
(
	VacancyID int identity primary key,
	VacancyName varchar(500) not null,
	ShopID int references Cms.Shops(ShopID),
	VacancyGender tinyint,
	VacancyAgeFrom int not null,
	VacancyAgeTo int,
	VacancyEducation varchar(max),
	VacancyExperience varchar(max),
	VacancyEmployment varchar(max),
	VacancyIncomeLevel varchar(500),
	VacancyRequirement varchar(max),
	VacancyFunctions varchar(max),
	VacancyNote varchar(max),
	VacancyContacts varchar(max),
	VacancyVisible bit not null	
)
go
IF OBJECT_ID('Cms.FeedbackRecipients', 'U') IS NOT NULL DROP TABLE Cms.FeedbackRecipients
go
create table Cms.FeedbackRecipients
(
	RecipientID int identity primary key,
	RecipientName varchar(250) not null,
	RecipientEmail varchar(250) not null,
	RecipientVisible bit not null	
)

GO

---------------------------------------------------------------------

update Cms.Vacancies set VacancyGender=0 where VacancyGender is null
go
alter table Cms.Vacancies alter column VacancyGender tinyint not null
go

update Cms.CatalogItems set CatalogItemPath='~/TecDoc/TecDocCatalog.aspx' where CatalogItemPath='~/tecdoc.aspx'
go

---------------------------------------------------------------------
IF OBJECT_ID('Cms.Brands', 'U') IS NOT NULL DROP TABLE Cms.Brands
CREATE TABLE Cms.Brands
(
	BrandID int identity CONSTRAINT PK_Brands primary key,
	VehicleType tinyint not null,
	[Name] VARCHAR(50) NOT NULL,
	Info VARCHAR(MAX) NULL,
	LogoFileID int references Cms.Files (FileID),
	AutoXPUrl varchar(500)
)
go

CREATE UNIQUE INDEX IX_Brands on Cms.Brands (VehicleType,[Name])

go
insert into Cms.Brands (VehicleType,Name,Info)
select 1,c.CatalogItemName,replace(t.TextItemBody,'Каталог запчастей','[Ссылка на AutoXP]') from Cms.CatalogItems c
inner join Cms.TextItems t on c.CatalogItemQueryString='ID='+cast(t.TextItemID as varchar(15))
where ParentItemID = (select CatalogItemID from Cms.CatalogItems where CatalogItemPath like '%onlinecatalogs%')
and CatalogItemName!='Общий каталог'
if @@error=0 begin
	delete from Cms.CatalogItems
	where ParentItemID = (select CatalogItemID from Cms.CatalogItems where CatalogItemPath like '%onlinecatalogs%')
end
go
---------------------------------------------------------------------

alter table Cms.Brands add PageTitle varchar(500)
alter table Cms.Brands add PageKeywords varchar(max)
alter table Cms.Brands add PageFooter varchar(max)
go

CREATE TABLE Cms.Manufacturers
(
	ManufacturerID int identity primary key, 
	[Name] VARCHAR(50) NOT NULL,
	Info VARCHAR(MAX) NULL,
	WebSiteUrl VARCHAR(256) NULL,
	LogoFileID int references Cms.Files (FileID),
	FolderID int references Cms.Folders (FolderID),
	ShowInCatalog bit not null default 0,
	PageTitle varchar(500),
	PageKeywords varchar(max),
	PageFooter varchar(max),
	CONSTRAINT UC_Manufacturers_Name UNIQUE (Name)
)

go
alter table Cms.CatalogItems add PageTitle varchar(500)
alter table Cms.CatalogItems add PageKeywords varchar(max)
alter table Cms.CatalogItems add PageDescription varchar(max)
alter table Cms.CatalogItems add PageFooter varchar(max)
go

update Cms.CatalogItems 
set PageTitle = CatalogItemTitle, 
	PageKeywords = CatalogItemKeywords, 
	PageDescription = CatalogItemDescription

go
alter table Cms.CatalogItems drop column CatalogItemTitle
alter table Cms.CatalogItems drop column CatalogItemKeywords
alter table Cms.CatalogItems drop column CatalogItemDescription
go

alter table Cms.Shops add ShopPriority int
go
update Cms.Shops set ShopPriority=0 where ShopPriority is null
go
alter table Cms.Shops alter column ShopPriority int not null
go

alter table Cms.Brands add PageDescription varchar(max)
alter table Cms.Manufacturers add PageDescription varchar(max)
go
--------------

alter table Cms.Brands add UrlCode varchar(50)
alter table Cms.Manufacturers add UrlCode varchar(50)

go
update Cms.Brands
set UrlCode=upper(substring(A,1,1))+lower(substring(A,2,len(A)-1))+replace(upper(substring(B,1,1))+lower(substring(B,2,len(B)-1)),'*','')
from Cms.Brands B
join (select 
BrandID,
A=case when charindex(' ',replace([Name],'-',' '))=0 then [Name] else ltrim(rtrim(substring(replace([Name],'-',' '),1,charindex(' ',replace([Name],'-',' '))))) end,
B=case when charindex(' ',replace([Name],'-',' '))=0 then '*' else ltrim(rtrim(substring(replace([Name],'-',' '),charindex(' ',replace([Name],'-',' ')),len([Name])))) end
from Cms.Brands) T on B.BrandID=T.BrandID

update Cms.Manufacturers
set UrlCode=upper(substring(A,1,1))+lower(substring(A,2,len(A)-1))+replace(upper(substring(B,1,1))+lower(substring(B,2,len(B)-1)),'*','')
from Cms.Manufacturers B
join (select 
ManufacturerID,
A=case when charindex(' ',replace([Name],'-',' '))=0 then [Name] else ltrim(rtrim(substring(replace([Name],'-',' '),1,charindex(' ',replace([Name],'-',' '))))) end,
B=case when charindex(' ',replace([Name],'-',' '))=0 then '*' else ltrim(rtrim(substring(replace([Name],'-',' '),charindex(' ',replace([Name],'-',' ')),len([Name])))) end
from Cms.Manufacturers) T on B.ManufacturerID=T.ManufacturerID
go


alter table Cms.Brands alter column UrlCode varchar(50) not null
alter table Cms.Manufacturers alter column UrlCode varchar(50) not null
go

create unique index IX_Brands_VehicleType_UrlCode on Cms.Brands (VehicleType,UrlCode)
create unique index IX_Manufacturers_UrlCode on Cms.Manufacturers (UrlCode)
go

---2009-12-07---

alter table Cms.Shops add StoreId varchar(50)
go
create index IX_Shops_StoreId on Cms.Shops (StoreId)
go
---2009-12-11---

alter table Cms.Employees add EmpPriority int
go
update Cms.Employees set EmpPriority=0 where EmpPriority is null
go
alter table Cms.Employees alter column EmpPriority int not null

---2009-12-17---

alter table Cms.Shops add ShopMetro varchar(max)
---2009-12-24---

go
alter table Cms.TextItems drop constraint PK_TextItems
go
CREATE TABLE Cms.Tmp_TextItems
	(
	TextItemID varchar(50) NOT NULL primary key,
	TextItemHeader varchar(500) NOT NULL,
	TextItemBody varchar(MAX) NOT NULL
	)
GO
IF EXISTS(SELECT * FROM Cms.TextItems)
	 EXEC('INSERT INTO Cms.Tmp_TextItems (TextItemID, TextItemHeader, TextItemBody)
		SELECT TextItemID, TextItemHeader, TextItemBody FROM Cms.TextItems')
GO
DROP TABLE Cms.TextItems
GO
EXECUTE sp_rename N'Cms.Tmp_TextItems', N'TextItems', 'OBJECT' 

go
alter table Cms.TextItems add TextItemFixed bit not null default 0

go
insert into Cms.TextItems (TextItemID,TextItemHeader,TextItemBody,TextItemFixed) values ('RetailContractTerms.Text','Условия договора (текст для отображения на последнем шаге оформления заказа)','[...]',1)
insert into Cms.TextItems (TextItemID,TextItemHeader,TextItemBody,TextItemFixed) values ('RetailContractTerms.Link','Условия договора (ссылка для отображения на последнем шаге оформления заказа)','<p><a href="http://rmsauto-dev-work.corp.parking.ru/Files/243.ashx">Cкачать договор</a></p>',1)
go

--2009-12-29--


IF OBJECT_ID('Cms.SeoPartsCatalogItems', 'U') IS NOT NULL DROP TABLE Cms.SeoPartsCatalogItems
GO
CREATE TABLE Cms.SeoPartsCatalogItems
(
	ID INT IDENTITY PRIMARY KEY,
	ParentID INT REFERENCES Cms.SeoPartsCatalogItems (ID),
	[Name] VARCHAR(100) NOT NULL,
	Body VARCHAR(MAX) NULL,
	UrlCode VARCHAR(50) NOT NULL,
	Visible BIT NOT NULL,
	
	PageTitle VARCHAR(500),
	PageKeywords VARCHAR(MAX),
	PageDescription VARCHAR(MAX),
	PageFooter VARCHAR(MAX)
)
create unique index IX_SeoPartsCatalogItems_ParentID_UrlCode on Cms.SeoPartsCatalogItems (ParentID,UrlCode)

create unique index IX_CatalogItems_ParentID_UrlCode on Cms.CatalogItems (ParentItemID,CatalogItemCode)

go
--2010-01-19--
alter table Cms.SeoPartsCatalogItems add Priority int
alter table Cms.SeoPartsCatalogItems add IsServicePage bit
go
update Cms.SeoPartsCatalogItems set IsServicePage=0
go
alter table Cms.SeoPartsCatalogItems alter column IsServicePage bit not null
go
alter table Cms.CatalogItems add PageBody varchar(max)
go
--2010-01-29--

create table Cms.SeoTecDocTextTemplates
(
	ID int identity primary key,
	TextType tinyint not null,
	TextTemplate varchar(1000) not null
)

--2010-02-24--

create table Cms.SeoTecDocTexts
(
	PageUrl varchar(100) primary key with (IGNORE_DUP_KEY=ON),
	PageTitleID int not null references Cms.SeoTecDocTextTemplates (ID),
	PageFooterID int not null references Cms.SeoTecDocTextTemplates (ID)
)

--2010-03-01--

alter table Cms.NewsItems add IconFileID int references Cms.Files (FileID)
alter table Cms.CatalogItems add IsServicePage bit not null default 0

--2010-03-22--

create table Cms.SparePartGroups
(
	SparePartGroupID int primary key,
	GroupName varchar(200) not null,
	BackgroundColor varchar(6) not null,
	Visible bit not null
)

--2010-04-21--


----2010-06-29---
INSERT INTO Cms.TextItems VALUES ('OrderLineTrackingAlert.BlockMailFooter', 'Текстовый блок в шаблоне-уведомлении',
'<p>&#160;</p>
<table style="background-color: #ffe076; width: 100%; height: 40px;" border="0" align="left">
<tbody>
<tr>
<td>Новости:</td>
<td><a href="http://rmsauto.ru/About/News/80.aspx">Высококачественная продукция компании GATES</a></td>
</tr>
<tr>
<td>Перейти:</td>
<td><a href="http://rmsauto.ru/Login.aspx?ReturnUrl=%2fCabinet%2fOrders%2fOrderDetails.aspx%3fID%3d137831%26back_url%3d%252fCabinet%252fOrders.aspx%253fview%253d2%2526sort%253d1%2526start%253d0%2526size%253d10">Личный кабинет</a></td>
</tr>
</tbody>
</table>', 'True');
GO

----2010-07-05---
INSERT INTO Cms.TextItems VALUES ('VINRequestRetail.Text', 'Нет доступа к запросам по VIN',
'По VIN запросам просьба обращаться непосредственно к менеджерам оптового отдела', 'True');
GO

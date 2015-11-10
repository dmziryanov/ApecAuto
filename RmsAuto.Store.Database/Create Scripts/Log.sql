create table SearchSparePartsLog (
	SearchDate datetime not null,
	PartNumber varchar(50) not null,
	ClientIP varchar(15) not null
)
GO
create clustered index IC_SearchSparePartsLog ON SearchSparePartsLog
(
	SearchDate asc
)
GO
alter table SearchSparePartsLog add Manufacturer varchar(50)

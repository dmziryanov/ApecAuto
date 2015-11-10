
declare @Clients table
(
	[Login] varchar(20) not null,
	PasswordBytes varbinary(1024),
	ClientID varchar(50) not null
)
-- по инсерту на каждого клиента
insert @Clients ([Login], PasswordBytes, ClientID) 
	values ('Login1', HashBytes('MD5','Password'), 'HW_ClientID1')  

insert dbo.Users (Username, [Password], UserRole, AcctgID)
select 
	[Login],
	cast(N'' as xml).value('xs:base64Binary(xs:hexBinary(sql:column("PasswordBytes")))', 'varchar(50)'),
	0,
	ClientID 
from @Clients 



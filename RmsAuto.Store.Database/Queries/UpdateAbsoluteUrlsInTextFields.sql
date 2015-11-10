declare @old varchar(100), @new varchar(100)
set @old = 'http://rmsauto-dev.corp.parking.ru'
set @new = 'http://rmsauto-dev-work.corp.parking.ru'

update Cms.TextItems set TextItemBody = REPLACE( TextItemBody, @old, @new )
update Cms.Brands set Info = REPLACE( Info, @old, @new )
update Cms.Manufacturers set Info = REPLACE( Info, @old, @new )
update Cms.Shops set ShopMetro = REPLACE( ShopMetro, @old, @new )
update Cms.Shops set ShopAddress = REPLACE( ShopAddress, @old, @new )

 
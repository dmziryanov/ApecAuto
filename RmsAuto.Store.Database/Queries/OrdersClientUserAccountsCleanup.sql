delete dbo.OrderLineStatusChanges
delete dbo.OrderLines
delete dbo.Orders

delete dbo.ShoppingCartItems

delete dbo.UserProfileEntries
from dbo.UserProfileEntries e join dbo.Users u ON e.UserID = u.UserID  
where UserRole = 0

delete dbo.Users where UserRole = 0

delete from Vin.VinRequestItems
delete from Vin.VinRequests
delete from Vin.UserGarageCars
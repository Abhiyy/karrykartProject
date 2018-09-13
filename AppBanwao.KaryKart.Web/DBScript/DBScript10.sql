select * from [order] where useriD='7BE0A638-C7C7-4E05-B5CF-A1CAF6A40577'
select * from Cart
select * from OrderProduct
select * from Payment
select * from GuestUserDetail
select * from ImportantValues
select UserID from [order] where UserID in (select UserID from Users)
select * from Users

select * from Cart


Alter table [order]
Add PlaceOn Datetime null

Update [Order]
set PlaceOn = GETDATE()

Alter table [order]
Add DeliveredOn Datetime null

Update [Order]
set DeliveredOn = GETDATE()

Alter table [Order]
add GuestCheckout bit

Alter table [Order]
add DeliveryAddressID int
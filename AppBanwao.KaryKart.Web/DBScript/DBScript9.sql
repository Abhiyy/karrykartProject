--select * from Users

--select * from [Users]
--select * from [UserDetail]
--select * from UserAddressDetail

--select * from refCity
--select * from refState
--select * from Country

Insert into refState values
('Delhi')

Insert into refCity values
('New Delhi')

--use karrykart
--select * from [Order]

ALTER TABLE [ORDER] 
ADD PaymentID uniqueidentifier

CREATE TABLE [dbo].[Payment]
(
	[ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Reference] VARCHAR(50) NULL, 
    [Amount] DECIMAL(9, 2) NULL, 
    [Type] INT NULL, 
    [IsSuccessful] BIT NULL 
)

ALTER TABLE [dbo].[ORDER] 
  ADD CONSTRAINT [FK_Order_Payment] 
  FOREIGN KEY ([PaymentID]) 
  REFERENCES[dbo].[Payment] ([ID])
  ON DELETE CASCADE;

ALTER TABLE [dbo].[ORDER] 
ADD [Status] int;

Update [order]
set [Status]=1

CREATE TABLE ImportantValues
(
ID int Primary Key identity(1,1),
[Description] varchar(50),
[Value] int,
[Type] varchar(50)
)

Insert into ImportantValues
values
('Placed',1,'OrderStatus'),
('Confirmed',2,'OrderStatus'),
('Dispatched',3,'OrderStatus'),
('In-Transit',4,'OrderStatus'),
('Delivered',5,'OrderStatus')

CREATE TABLE [dbo].[GuestUserDetail]
(
	[ID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [FirstName] VARCHAR(50) NULL, 
    [LastName] VARCHAR(50) NULL, 
    [Phone] VARCHAR(20) NULL, 
    [EmailAddress] VARCHAR(255) NULL, 
    [AddressLine1] VARCHAR(250) NULL, 
    [AddressLine2] VARCHAR(250) NULL, 
    [LandMark] VARCHAR(100) NULL, 
    [CityID] INT NULL, 
    [StateID] INT NULL, 
    [CountryID] INT NULL, 
    [Pincode] VARCHAR(10) NULL
)



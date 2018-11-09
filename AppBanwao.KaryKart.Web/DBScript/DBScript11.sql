--DROP TABLE [dbo].[PaymentTransaction]

CREATE TABLE [dbo].[PaymentTransaction]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TxnID] UNIQUEIDENTIFIER NULL, 
    [CartID] UNIQUEIDENTIFIER NULL, 
    [UserID] UNIQUEIDENTIFIER NULL, 
    [TransactionStatus] VARCHAR(50) NULL,
	[GuestCheckout] bit NULL,
	[AddressID] int NULL
	 
)

DROP TABLE [dbo].[Panel]

CREATE TABLE [dbo].[Panel]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] varchar(50), 
    [Title] varchar(100), 
    [Type] int, 
    [For] VARCHAR(20) NULL,
	[ItemOrder] int NULL,
	[Active] bit NULL,
	[html] nvarchar(max) NULL
)

Insert into Panel values
( 'Category Menu', 'Shop by categories', 1,'Mobile', 1, 1,NULL),
( 'Offer', 'Offers', 2,'Mobile', 2, 1,NULL)



DROP TABLE [dbo].[PanelItem]

CREATE TABLE [dbo].[PanelItem]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[PanelID] int,
    [Title] varchar(100), 
    [ItemType] int, 
    [Active] bit NULL,
	[html] nvarchar(max) NULL,
	[ImageLink] varchar(255), 
	[ItemID] varchar(255),
	[Link] varchar(255)
	FOREIGN KEY ([PanelID]) REFERENCES Panel(ID)
)

select * from Panel
select * from PanelItem
select * from Category
select * from Product
select * from ProductImage
Insert into  [dbo].[PanelItem] values
(1,'Cakes', 1,1,null,'~/Images/cakes.jpg',1,null),
(1,'Pastries', 1,1,null,'~/Images/pastries.jpg',2,null),
(1,'Pastries', 1,1,null,'~/Images/pastries.jpg',3,null)

Insert into  [dbo].[PanelItem] values
(2,'Cakes', 2,1,null,'~/Images/cake.jpg',1,null),
(2,'Pastries', 2,1,null,'~/Images/pastries.jpg',2,null),
(2,'Food', 2,1,null,'~/Images/food.jpg',3,null)


Insert into Panel values
( 'Tabs', 'Tabs', 3,'Mobile', 3, 1,NULL)

  
Insert into  [dbo].[PanelItem] values
(3,'Gits', 3,1,null,'~/Images/cake.jpg',4,null),
(3,'Jewellery', 3,1,null,'~/Images/pastries.jpg',5,null)


     

select * from Panel






select * from [Order]

select * from UserLogin

delete from UserLogin

delete from PaymentTransaction

select * from PaymentTransaction
select * from Users

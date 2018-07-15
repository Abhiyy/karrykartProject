CREATE TABLE [dbo].[Cart] (
    [ID]         UNIQUEIDENTIFIER NOT NULL,
    [CartUserID] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED ([ID] ASC)
);

CREATE TABLE [dbo].[CartProduct] (
    [ID]        INT              IDENTITY (1, 1) NOT NULL,
    [CartID]    UNIQUEIDENTIFIER NULL,
    [ProductID] UNIQUEIDENTIFIER NULL,
    [Quantity]  INT              NULL,
    CONSTRAINT [PK_CartProduct] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FK_CartProducts_Cart] FOREIGN KEY ([CartID]) REFERENCES [dbo].[Cart] ([ID]),
    CONSTRAINT [FK_CartProduct_ProductID] FOREIGN KEY ([ProductID]) REFERENCES [dbo].[Product] ([ProductID])
);

CREATE TABLE [dbo].[Order] (
    [ID]     UNIQUEIDENTIFIER NOT NULL,
    [CartID] UNIQUEIDENTIFIER NULL,
    [UserID] UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);



CREATE TABLE [dbo].[OrderProduct]
(
	[Id] INT PRIMARY KEY NOT NULL IDENTITY, 
    [OrderID] UNIQUEIDENTIFIER NULL, 
    [ProductID] UNIQUEIDENTIFIER NULL, 
    [Quantity] INT NULL, 
    CONSTRAINT [FK_OrderProduct_Product] FOREIGN KEY ([ProductID]) REFERENCES [Product]([ProductID]), 
    CONSTRAINT [FK_OrderProduct_Order] FOREIGN KEY ([OrderID]) REFERENCES [Order]([Id]) 
)


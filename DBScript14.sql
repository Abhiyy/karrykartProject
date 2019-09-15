select * from ImportantValues

select * from Roles


Insert Into ImportantValues values
('Cancel',6,'OrderStatus')
Insert into Roles 
values ('Seller')

GO

/****** Object:  Table [dbo].[OrderJourney]    Script Date: 09/14/2019 20:52:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[OrderJourney](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [uniqueidentifier] NULL,
	[OrderStatusID] [int] NULL,
	[OrderStatus] [varchar](15) NULL,
	[CreatedAt] [datetime] NULL,
	[CreatedBy] [varchar](155) NULL,
 CONSTRAINT [PK_OrderJourney] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[OrderJourney]  WITH CHECK ADD  CONSTRAINT [FK_OrderJourney_Order1] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([ID])
GO

ALTER TABLE [dbo].[OrderJourney] CHECK CONSTRAINT [FK_OrderJourney_Order1]
GO

GO

/****** Object:  Table [dbo].[UserAlert]    Script Date: 09/14/2019 21:08:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[UserAlert](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [uniqueidentifier] NULL,
	[Message] [varchar](400) NULL,
	[Seen] [bit] NULL,
	[CreatedAt] [datetime] NULL,
 CONSTRAINT [PK_UserAlert] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[UserAlert]  WITH CHECK ADD  CONSTRAINT [FK_UserAlert_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO

ALTER TABLE [dbo].[UserAlert] CHECK CONSTRAINT [FK_UserAlert_Users]
GO


select * from [OrderJourney]

select * from [Order] where ID = '621fae22-56c3-4189-8119-dc997d7a41c3'

USE [Karrykart]
GO

/****** Object:  Table [dbo].[VideoLog]    Script Date: 01/28/2019 12:56:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[VideoLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[Description] [nvarchar](400) NULL,
	[Transcript] [nvarchar](max) NULL,
	[OwnerID] [uniqueidentifier] NULL,
	[Link] [nvarchar](500) NULL,
	[LikeCount] [bigint] NULL,
	[ViewCount] [bigint] NULL,
 CONSTRAINT [PK_VideoLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[VideoLog]  WITH CHECK ADD  CONSTRAINT [FK_VideoLog_Users] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[Users] ([UserID])
GO

ALTER TABLE [dbo].[VideoLog] CHECK CONSTRAINT [FK_VideoLog_Users]
GO



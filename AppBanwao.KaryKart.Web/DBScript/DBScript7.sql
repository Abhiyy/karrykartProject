CREATE TABLE [dbo].[UserLogin]
(
	[Id] INT NOT NULL PRIMARY KEY identity,
	[UserID] uniqueidentifier,
	[Token] uniqueidentifier,
	[TokenExpiry] datetime,
	[LoginTime] datetime, 
    CONSTRAINT [FK_UserLogin_Users] FOREIGN KEY ([UserID]) REFERENCES [Users]([UserID]) 
)

select * from userlogin
delete from UserLogin
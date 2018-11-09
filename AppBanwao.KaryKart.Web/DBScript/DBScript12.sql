
Insert into ImportantValues values
('PageHit','0','WebOject')


Insert into ImportantValues values
('Cancel',6,'OrderStatus')

GO

/****** Object:  StoredProcedure [dbo].[UpdatePageHit]    Script Date: 11/05/2018 08:25:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UpdatePageHit]
	AS
	declare @currentValue as int
	select @currentValue = CAST(Value AS int) from ImportantValues 
	where [Description] = 'PageHit' and [Type] = 'WebOject'
	
	update ImportantValues
	set Value = @currentValue + 1
	where [Description] = 'PageHit' and [Type] = 'WebOject'

RETURN 0
GO


GO

/****** Object:  StoredProcedure [dbo].[GetDashBoardCards]    Script Date: 11/05/2018 08:24:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		Abhilash Sharma
-- Create date: 05-Nov-2018
-- Description:	It gives the data for the admin cards.
-- =============================================
CREATE PROCEDURE [dbo].[GetDashBoardCards]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @dashboard TABLE
	(
	 Name varchar(30),
	 CardCount int
	)

	Insert into @dashboard (Name, CardCount)
	select 
	'Users' as Name, count(UserID) as CardCount from Users

	Insert into @dashboard (Name, CardCount)
	select 
	'New Orders' as Name, count(ID) as CardCount from [Order] 
	Where [Status] = 1

	Insert into @dashboard (Name, CardCount)
	select 
	'Website Hit' as Name, Value as CardCount from [ImportantValues] 
	where [Description] = 'PageHit' and [Type] = 'WebOject'

	Insert into @dashboard (Name, CardCount)
	select 
	'Active Products' as Name, Count(ProductID) as CardCount from [Product]
	where Active=1

	select * from @dashboard 
END


GO



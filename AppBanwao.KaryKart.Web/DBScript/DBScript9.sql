select * from Users

select * from [Users]
select * from [UserDetail]
select * from UserAddressDetail

select * from refCity
select * from refState
select * from Country

Insert into refState values
('Delhi')

Insert into refCity values
('New Delhi')

use karrykart
select * from [Order]

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


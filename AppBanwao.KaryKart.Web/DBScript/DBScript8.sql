ALTER TABLE [dbo].[CartProduct] DROP CONSTRAINT [FK_CartProducts_Cart]

ALTER TABLE [dbo].[CartProduct] 
  ADD CONSTRAINT [FK_CartProducts_Cart] 
  FOREIGN KEY ([CartID]) 
  REFERENCES[dbo].[Cart] ([ID])
  ON DELETE CASCADE;




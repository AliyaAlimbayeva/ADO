﻿CREATE PROCEDURE [dbo].[spClearDB]
AS
 BEGIN
	TRUNCATE TABLE Orders

	IF (OBJECT_ID('FK_Orders_Products', 'F') IS NOT NULL)
	BEGIN
    ALTER TABLE Orders DROP CONSTRAINT FK_Orders_Products
	END

	TRUNCATE TABLE Products

	ALTER TABLE Orders
	ADD CONSTRAINT FK_Orders_Products
	FOREIGN KEY (ProductId) REFERENCES Products(Id);
 END
GO

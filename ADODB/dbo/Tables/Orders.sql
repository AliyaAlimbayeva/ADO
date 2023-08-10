CREATE TABLE [dbo].[Orders]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Status] NVARCHAR(10) CHECK ([Status] IN ('NotStarted', 'Loading', 'InProgress', 'Arrived', 'Unloading', 'Cancelled', 'Done')), 
    [CreatedDate] DATETIME NOT NULL, 
    [UpdatedDate] DATETIME NULL, 
    [ProductId] INT NOT NULL, 
    CONSTRAINT [FK_Orders_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id])
)

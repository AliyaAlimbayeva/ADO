CREATE TABLE [dbo].[Products]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NULL, 
    [Description] NVARCHAR(150) NULL, 
    [Weight] NUMERIC(5, 2) NULL, 
    [Height] NUMERIC(5, 2) NULL, 
    [Width] NUMERIC(5, 2) NULL, 
    [Length] NUMERIC(5, 2) NULL
)

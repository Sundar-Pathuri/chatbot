CREATE TABLE [dbo].[Categories]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Product] NVARCHAR(50) NOT NULL, 
    [SubProduct] NVARCHAR(MAX) NOT NULL
)

CREATE TABLE [dbo].[SubCategory]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [SubProduct] NVARCHAR(MAX) NOT NULL, 
    [SubCategory] NVARCHAR(MAX) NOT NULL
)

CREATE TABLE [dbo].[links]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [SubCategory] NVARCHAR(MAX) NOT NULL, 
    [Image] NVARCHAR(MAX) NOT NULL, 
    [link] NVARCHAR(MAX) NOT NULL
)

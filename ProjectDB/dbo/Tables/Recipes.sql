CREATE TABLE [dbo].[Recipes]
(
	    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Recipes_Column]
    ON [dbo].[Recipes]([Name] ASC);


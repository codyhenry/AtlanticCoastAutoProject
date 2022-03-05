CREATE TABLE [dbo].[Ingredients]
(
    [Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Name]     VARCHAR (50) NOT NULL,
    [Category] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Ingredients_Column]
    ON [dbo].[Ingredients]([Category] ASC);
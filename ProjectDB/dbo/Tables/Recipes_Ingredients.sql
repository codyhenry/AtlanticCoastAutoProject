CREATE TABLE [dbo].[Recipes_Ingredients]
(
    [Id]   INT IDENTITY (1, 1) NOT NULL,
    [R_Id] INT NOT NULL,
    [I_Id] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
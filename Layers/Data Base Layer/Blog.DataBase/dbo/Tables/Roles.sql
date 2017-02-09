CREATE TABLE [dbo].[Roles] (
    [IdRole] INT           IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (256) NOT NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([IdRole] ASC)
);



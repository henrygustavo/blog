CREATE TABLE [dbo].[CategorySetting] (
    [IdCategorySetting] INT          IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_CategorySetting] PRIMARY KEY CLUSTERED ([IdCategorySetting] ASC)
);



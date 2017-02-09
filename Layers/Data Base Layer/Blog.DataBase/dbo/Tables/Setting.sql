CREATE TABLE [dbo].[Setting] (
    [IdSetting]         INT          IDENTITY (1, 1) NOT NULL,
    [Name]              VARCHAR (50) NOT NULL,
    [ParamValue]        VARCHAR (50) NULL,
    [IdCategorySetting] INT          NOT NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([IdSetting] ASC),
    CONSTRAINT [FK_Setting_CategorySetting] FOREIGN KEY ([IdCategorySetting]) REFERENCES [dbo].[CategorySetting] ([IdCategorySetting])
);



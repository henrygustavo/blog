CREATE TABLE [dbo].[PersonalInformation] (
    [IdPersonalInformation] INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]             VARCHAR (50)  NOT NULL,
    [LastName]              VARCHAR (50)  NOT NULL,
    [SiteName]              VARCHAR (50)  NOT NULL,
    [Email]                 VARCHAR (50)  NOT NULL,
    [Country]               VARCHAR (50)  NOT NULL,
    [PhoneNumber]           VARCHAR (20)  NOT NULL,
    [IdPhoto]               VARCHAR (MAX) NULL,
    [Description]           VARCHAR (MAX) NULL,
    [FaceBook]              VARCHAR (100) NULL,
    [Twitter]               VARCHAR (100) NULL,
    [GooglePlus]            VARCHAR (100) NULL,
    [CreationDate]          DATETIME      NOT NULL,
    [LastActivityDate]      DATETIME      NOT NULL,
    [CreationIdUser]        INT           NOT NULL,
    [LastActivityIdUser]    INT           NOT NULL,
    CONSTRAINT [PK_PersonalInformation] PRIMARY KEY CLUSTERED ([IdPersonalInformation] ASC)
);






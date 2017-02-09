CREATE TABLE [dbo].[Tag] (
    [IdTag]              INT          IDENTITY (1, 1) NOT NULL,
    [Name]               VARCHAR (50) NOT NULL,
    [State]              INT          NOT NULL,
    [CreationDate]       DATETIME     NOT NULL,
    [LastActivityDate]   DATETIME     NOT NULL,
    [CreationIdUser]     INT          NOT NULL,
    [LastActivityIdUser] INT          NOT NULL,
    CONSTRAINT [PK_Tag] PRIMARY KEY CLUSTERED ([IdTag] ASC)
);


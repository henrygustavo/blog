CREATE TABLE [dbo].[BlogEntry] (
    [IdBlogEntry]        INT           IDENTITY (1, 1) NOT NULL,
    [Header]             VARCHAR (200) NOT NULL,
    [HeaderUrl]          VARCHAR (200) NOT NULL,
    [Author]             VARCHAR (100) NOT NULL,
    [ShortContent]       VARCHAR (MAX) NOT NULL,
    [Content]            VARCHAR (MAX) NOT NULL,
    [State]              INT           NOT NULL,
    [CreationDate]       DATETIME      NOT NULL,
    [LastActivityDate]   DATETIME      NOT NULL,
    [CreationIdUser]     INT           NOT NULL,
    [LastActivityIdUser] INT           NOT NULL,
    CONSTRAINT [PK_BlogEntry] PRIMARY KEY CLUSTERED ([IdBlogEntry] ASC)
);


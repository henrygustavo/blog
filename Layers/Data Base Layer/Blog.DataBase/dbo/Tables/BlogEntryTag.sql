CREATE TABLE [dbo].[BlogEntryTag] (
    [IdBlogEntryTag]     INT      IDENTITY (1, 1) NOT NULL,
    [IdBlogEntry]        INT      NOT NULL,
    [IdTag]              INT      NOT NULL,
    [CreationDate]       DATETIME NOT NULL,
    [LastActivityDate]   DATETIME NOT NULL,
    [CreationIdUser]     INT      NOT NULL,
    [LastActivityIdUser] INT      NOT NULL,
    CONSTRAINT [PK_BlogEntryTag] PRIMARY KEY CLUSTERED ([IdBlogEntryTag] ASC),
    CONSTRAINT [FK_BlogEntryTag_BlogEntry] FOREIGN KEY ([IdBlogEntry]) REFERENCES [dbo].[BlogEntry] ([IdBlogEntry]),
    CONSTRAINT [FK_BlogEntryTag_Tag] FOREIGN KEY ([IdTag]) REFERENCES [dbo].[Tag] ([IdTag])
);


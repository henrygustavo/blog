CREATE TABLE [dbo].[BlogEntryComment] (
    [IdBlogEntryComment] INT           IDENTITY (1, 1) NOT NULL,
    [IdBlogEntry]        INT           NOT NULL,
    [Name]               VARCHAR (50)  NOT NULL,
    [Comment]            VARCHAR (MAX) NOT NULL,
    [Email]              VARCHAR (50)  NOT NULL,
    [AdminPost]          BIT           NOT NULL,
    [CreationDate]       DATETIME      NOT NULL,
    CONSTRAINT [PK_BlogEntryComment] PRIMARY KEY CLUSTERED ([IdBlogEntryComment] ASC),
    CONSTRAINT [FK_BlogEntryComment_BlogEntry] FOREIGN KEY ([IdBlogEntry]) REFERENCES [dbo].[BlogEntry] ([IdBlogEntry])
);


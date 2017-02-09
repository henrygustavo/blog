CREATE TABLE [dbo].[UserLogins] (
    [LoginProvider] VARCHAR (128) NOT NULL,
    [ProviderKey]   VARCHAR (128) NOT NULL,
    [IdUser]        INT           NOT NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [IdUser] ASC),
    CONSTRAINT [FK_UserLogins_Users] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[Users] ([IdUser]) ON DELETE CASCADE
);



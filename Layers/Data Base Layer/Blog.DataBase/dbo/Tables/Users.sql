CREATE TABLE [dbo].[Users] (
    [IdUser]               INT            IDENTITY (1, 1) NOT NULL,
    [UserName]             VARCHAR (256)  NOT NULL,
    [Email]                VARCHAR (256)  CONSTRAINT [DF__Users__Email__02FC7413] DEFAULT (NULL) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEndDateUtc]    DATETIME       CONSTRAINT [DF__Users__LockoutEn__03F0984C] DEFAULT (NULL) NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [CreationDate]         DATETIME       NOT NULL,
    [LastActivityDate]     DATETIME       NOT NULL,
    [Disabled]             BIT            NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([IdUser] ASC)
);

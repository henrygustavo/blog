CREATE TABLE [dbo].[UserClaims] (
    [UserClaimId] INT            IDENTITY (1, 1) NOT NULL,
    [IdUser]      INT            NOT NULL,
    [ClaimType]   NVARCHAR (MAX) NULL,
    [ClaimValue]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY CLUSTERED ([UserClaimId] ASC),
    CONSTRAINT [FK_UserClaims_Users] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[Users] ([IdUser]) ON DELETE CASCADE
);



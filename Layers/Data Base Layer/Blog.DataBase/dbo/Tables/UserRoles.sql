CREATE TABLE [dbo].[UserRoles] (
    [IdUser] INT NOT NULL,
    [IdRole] INT NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([IdUser] ASC, [IdRole] ASC),
    CONSTRAINT [FK_UserRoles_Roles] FOREIGN KEY ([IdRole]) REFERENCES [dbo].[Roles] ([IdRole]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY ([IdUser]) REFERENCES [dbo].[Users] ([IdUser]) ON DELETE CASCADE
);



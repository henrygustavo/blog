CREATE TABLE [dbo].[GoogleApiDataStore] (
    [IdGoogleApiDataStore] INT           IDENTITY (1, 1) NOT NULL,
    [RefreshToken]         VARCHAR (MAX) NOT NULL,
    [UserName]             VARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_GoogleApiDataStore] PRIMARY KEY CLUSTERED ([IdGoogleApiDataStore] ASC)
);



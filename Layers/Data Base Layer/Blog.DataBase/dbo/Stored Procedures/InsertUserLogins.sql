
CREATE PROCEDURE [dbo].[InsertUserLogins]
(
	@IdUser int,
	@LoginProvider varchar(128),
	@ProviderKey  varchar(128)
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO [dbo].[UserLogins]
           ([LoginProvider]
           ,[ProviderKey]
           ,[IdUser])
     VALUES
           (@LoginProvider
           ,@ProviderKey 
           ,@IdUser)


	SET NOCOUNT OFF
END

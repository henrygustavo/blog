
CREATE PROCEDURE [dbo].[DeleteUserLogins]
(
	@IdUser int,
	@LoginProvider varchar(128),
	@ProviderKey  varchar(128)
)
AS
BEGIN
	SET NOCOUNT ON

	Delete from UserLogins where IdUser = @IdUser and LoginProvider = @LoginProvider and ProviderKey = @ProviderKey


	SET NOCOUNT OFF
END


CREATE PROCEDURE [dbo].[GetIdUserByUserLogins]
(
	@LoginProvider varchar(128),
	@ProviderKey  varchar(128)
)
AS
BEGIN
	SET NOCOUNT ON

	Select IdUser from UserLogins where LoginProvider = @LoginProvider and ProviderKey = @ProviderKey 

	SET NOCOUNT OFF
END

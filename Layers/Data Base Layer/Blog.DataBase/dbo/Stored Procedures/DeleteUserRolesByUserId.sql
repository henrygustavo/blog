
CREATE PROCEDURE [dbo].[DeleteUserRolesByIdUser]
(
	@IdUser int
)
AS
BEGIN
	SET NOCOUNT ON

	Delete from UserRoles where IdUser = @IdUser

	SET NOCOUNT OFF
END

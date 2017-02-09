
CREATE PROCEDURE [dbo].[GetUserRolesByIdUser]
(
	@IdUser int
)
AS
BEGIN
	SET NOCOUNT ON

	Select Roles.Name from UserRoles, Roles where UserRoles.IdUser = @IdUser and UserRoles.IdRole = Roles.IdRole

	SET NOCOUNT OFF
END

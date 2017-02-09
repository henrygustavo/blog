
CREATE PROCEDURE [dbo].[GetUserById]
(
	@IdUser int
)
AS
BEGIN
	SET NOCOUNT ON

	select 
		users.IdUser as Id,
		users.UserName, 
		users.Email,users.LockoutEnabled,
		users.PhoneNumber,
		users.PhoneNumberConfirmed,
		users.Disabled, 
		userRoles.IdRole from Users users
		left join UserRoles userRoles on users.IdUser = userRoles.IdUser 
		where users.IdUser =@IdUser

	SET NOCOUNT OFF
END

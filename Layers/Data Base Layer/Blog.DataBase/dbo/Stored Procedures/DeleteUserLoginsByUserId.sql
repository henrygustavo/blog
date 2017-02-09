
CREATE PROCEDURE [dbo].[DeleteUserLoginsByIdUser]
(
	@IdUser int
)
AS
BEGIN
	SET NOCOUNT ON

	Delete from UserLogins where IdUser = @IdUser


	SET NOCOUNT OFF
END

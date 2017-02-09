
CREATE PROCEDURE [dbo].[GetUserLoginsByIdUser]
(
	@IdUser int
)
AS
BEGIN
	SET NOCOUNT ON

	Select * from UserLogins where IdUser = @IdUser

	SET NOCOUNT OFF
END

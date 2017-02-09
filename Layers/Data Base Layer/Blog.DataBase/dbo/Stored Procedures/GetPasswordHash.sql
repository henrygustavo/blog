
CREATE PROCEDURE [dbo].[GetPasswordHash]
(
	@IdUser int
)
AS
BEGIN
	SET NOCOUNT ON

	Select PasswordHash from Users where IdUser = @IdUser

	SET NOCOUNT OFF
END

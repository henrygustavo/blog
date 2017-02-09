
CREATE PROCEDURE [dbo].[DeleteUserClaimsByIdUser]
(
	@IdUser int
)
AS
BEGIN
	SET NOCOUNT ON

	Delete from UserClaims where  IdUser = @IdUser

	SET NOCOUNT OFF
END

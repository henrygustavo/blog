
CREATE PROCEDURE [dbo].[GetUserClaimsByIdUser]
(
	@IdUser int
)
AS
BEGIN
	SET NOCOUNT ON

	Select * from UserClaims where IdUser = @IdUser

	SET NOCOUNT OFF
END

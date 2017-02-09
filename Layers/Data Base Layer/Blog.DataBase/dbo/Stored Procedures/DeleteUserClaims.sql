
CREATE PROCEDURE [dbo].[DeleteUserClaims]
(
	@IdUser int,
	@ClaimType nvarchar(max),
	@ClaimValue  nvarchar(max)
)
AS
BEGIN
	SET NOCOUNT ON

	Delete from UserClaims where IdUser = @IdUser and ClaimValue = @ClaimValue and ClaimType = @ClaimType


	SET NOCOUNT OFF
END

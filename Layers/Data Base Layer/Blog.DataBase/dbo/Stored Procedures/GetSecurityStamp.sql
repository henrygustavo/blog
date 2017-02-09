
CREATE PROCEDURE [dbo].[GetSecurityStamp]
(
	@IdUser int
)
AS
BEGIN
	SET NOCOUNT ON

	Select SecurityStamp from Users where  IdUser = @IdUser

	SET NOCOUNT OFF
END


CREATE PROCEDURE [dbo].[InsertUserRoles]
(
	@IdUser int,
	@IdRole int
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO [dbo].[UserRoles]
           ([IdUser]
           ,[IdRole])
     VALUES
           (@IdUser
           ,@IdRole)

	SET NOCOUNT OFF
END

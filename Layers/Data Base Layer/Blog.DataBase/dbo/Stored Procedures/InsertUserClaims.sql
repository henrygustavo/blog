
CREATE PROCEDURE [dbo].[InsertUserClaims]
(
	@IdUser int,
	@ClaimType nvarchar(max),
	@ClaimValue  nvarchar(max)
)
AS
BEGIN
	SET NOCOUNT ON

	INSERT INTO [dbo].[UserClaims]
           ([IdUser]
           ,[ClaimType]
           ,[ClaimValue])
     VALUES
           (@IdUser
           ,@ClaimType
           ,@ClaimValue)


	SET NOCOUNT OFF
END

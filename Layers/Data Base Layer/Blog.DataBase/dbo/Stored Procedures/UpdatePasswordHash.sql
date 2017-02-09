
CREATE PROCEDURE [dbo].[UpdatePasswordHash]
(
	@Id int,
	@PasswordHash varchar(max)
)
AS
BEGIN
	SET NOCOUNT ON

	Update Users set PasswordHash = @PasswordHash where IdUser = @Id

	SET NOCOUNT OFF
END

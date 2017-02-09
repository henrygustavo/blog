CREATE PROCEDURE [dbo].[TruncateGoogleApiDataStore]

AS
BEGIN
	SET NOCOUNT ON

	truncate table [dbo].[GoogleApiDataStore]

	SET NOCOUNT OFF
END

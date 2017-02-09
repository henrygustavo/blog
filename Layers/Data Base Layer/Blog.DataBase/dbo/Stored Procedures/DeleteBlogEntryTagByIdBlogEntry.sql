CREATE PROCEDURE [dbo].[DeleteBlogEntryTagByIdBlogEntry]
(
	@IdBlogEntry int
)
AS
BEGIN
	SET NOCOUNT ON

	Delete from BlogEntryTag where IdBlogEntry = @IdBlogEntry


	SET NOCOUNT OFF
END

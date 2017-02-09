CREATE PROCEDURE [dbo].[GetBlogEntryTagByIdBlogEntry]
(	
	@IdBlogEntry int
)
AS
BEGIN
	SET NOCOUNT ON

	select 
		[dbo].[Tag].Name,
		[dbo].[Tag].IdTag as 'Id'
		

	from [dbo].[BlogEntryTag]  inner join [dbo].[Tag]  on [BlogEntryTag].IdTag = [dbo].[Tag].IdTag

	where [dbo].[BlogEntryTag].IdBlogEntry = @IdBlogEntry

	SET NOCOUNT OFF
END


CREATE VIEW [dbo].[BlogEntryView]
AS

SELECT 
    BlogEntry.IdBlogEntry,
    BlogEntry.header,
	 BlogEntry.headerUrl,
	BlogEntry.Content,
	BlogEntry.ShortContent,
    BlogEntry.author,
	BlogEntry.CreationDate,
    BlogEntry.state,
    Setting.name AS 'StateName',
    isnull(comments.countComments, 0) as TotalComments,

    tags = STUFF( (SELECT ',' + BlogEntryTagView.name 
                             FROM dbo.BlogEntryTagView
							 where  BlogEntryTagView.IdBlogEntry = BlogEntry.IdBlogEntry
                             ORDER BY BlogEntryTagView.name
                             FOR XML PATH('')), 
                            1, 1, '')
FROM
    BlogEntry BlogEntry
        INNER JOIN
    [dbo].[Setting] Setting ON BlogEntry.state = Setting.IdSetting

        LEFT JOIN
        BlogEntryCommentView comments ON comments.IdBlogEntry = BlogEntry.IdBlogEntry
                             
CREATE VIEW BlogEntryCommentView
 AS
   SELECT 
          BlogEntryComment.IdBlogEntry, COUNT( BlogEntryComment.IdBlogEntry) AS countComments
   FROM
          BlogEntryComment
   GROUP BY  BlogEntryComment.IdBlogEntry
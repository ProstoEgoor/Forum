CREATE VIEW [dbo].[sort_answer] AS
SELECT * FROM [dbo].[answer]
WHERE [question_id] = 1
ORDER BY [create_date] asc, [rating] desc OFFSET 0 ROWS; 

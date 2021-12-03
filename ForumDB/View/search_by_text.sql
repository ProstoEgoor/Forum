CREATE VIEW [dbo].[search_by_text]
	AS SELECT * FROM [dbo].[question]
	WHERE [question_text] LIKE '%c#%';

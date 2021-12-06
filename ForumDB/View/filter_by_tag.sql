CREATE VIEW [dbo].[filter_by_tag]
	AS SELECT * FROM [dbo].[question]
	WHERE [question_id] IN (
		SELECT [question_id] FROM [dbo].[tag_in_question] AS tq
		WHERE tq.[tag_name] = 'faq'
	);

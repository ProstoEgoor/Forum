CREATE VIEW [dbo].[tag_frequency]
	AS SELECT [tag_name], COUNT(*) as frequency FROM [dbo].[tag_in_question]
	GROUP BY [tag_name];

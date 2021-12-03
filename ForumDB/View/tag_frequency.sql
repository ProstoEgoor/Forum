CREATE VIEW [dbo].[tag_frequency]
	AS SELECT [tag], COUNT(*) as frequency FROM [dbo].[tag_in_question]
	GROUP BY [tag];

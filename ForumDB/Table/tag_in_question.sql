CREATE TABLE [dbo].[tag_in_question]
(
	[question_id] INT NOT NULL,
	[tag_name] NVARCHAR(100) NOT NULL,
	PRIMARY KEY ([question_id], [tag_name]),
	FOREIGN KEY ([question_id]) REFERENCES [dbo].[question](question_id)
		ON UPDATE CASCADE
		ON DELETE CASCADE
)

CREATE TABLE [dbo].[answer]
(
	[answer_id] INT NOT NULL IDENTITY PRIMARY KEY,
	[question_id] INT NOT NULL,
	[create_date] DATETIME2 NOT NULL DEFAULT GETDATE(),
	[author_name] NVARCHAR(100) NOT NULL,
	[answer_text] NVARCHAR(max) NOT NULL,
	[vote_positive] INT NOT NULL DEFAULT 0 CHECK ([vote_positive] >= 0),
	[vote_negative] INT NOT NULL DEFAULT 0 CHECK ([vote_negative] >= 0),
	[rating] AS [vote_positive] - [vote_negative],

	FOREIGN KEY ([question_id]) 
		REFERENCES [dbo].[question](question_id) 
		ON UPDATE CASCADE 
		ON DELETE CASCADE
)

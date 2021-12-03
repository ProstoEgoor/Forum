CREATE TABLE [dbo].[question]
(
	[question_id] INT NOT NULL IDENTITY PRIMARY KEY,
	[create_date] DATETIME2 NOT NULL DEFAULT GETDATE(),
	[author_name] NVARCHAR(100) NOT NULL,
	[topic] NVARCHAR(1000) NOT NULL,
	[question_text] NVARCHAR(MAX) NOT NULL
)

USE [Forum]

DECLARE @Text NVARCHAR(max)
DECLARE @NewLine CHAR(2) = CHAR(13)+CHAR(10)
DECLARE @QuestionId INT

SET @Text = 'Когда я выполняю некоторый код, выбрасывается исключение NullReferenceException со следующим сообщением:' + @NewLine +
			'Object reference not set to an instance of an object.' + @NewLine + 
			'или' + @NewLine +
			'В экземпляре объекта не задана ссылка на объект.' + @NewLine + 
			'Что это значит, и как мне исправить код ?'
				
INSERT INTO [question] ([create_date], [author_name], [topic], [question_text])
VALUES ('20150401 10:18:00', 'Kyubey', 'Что такое NullReferenceException, и как мне исправить код?', @Text)

SET @QuestionId = (SELECT MAX([question_id]) FROM [question]);

INSERT INTO [tag_in_question] ([question_id], [tag_name])
VALUES	(@QuestionId, 'c#'),
		(@QuestionId, '.net'),
		(@QuestionId, 'vb.net'),
		(@QuestionId, 'faq'),
		(@QuestionId, 'c#-faq')

SET @Text = 'Вы пытаетесь воспользоваться чем-то, что равно null (или Nothing в VB.NET). Это означает, что либо вы присвоили это значение, либо вы ничего не присваивали.' + @NewLine +
			 @NewLine + 
			'Как и любое другое значение, null может передаваться от объекта к объекту, от метода к методу. Если нечто равно null в методе "А", вполне может быть, что метод "В" передал это значение в метод "А".'
				
INSERT INTO [answer] ([question_id], [create_date], [author_name], [answer_text], [vote_positive], [vote_negative])
VALUES (@QuestionId, '20150401 10:18:00', 'Kyubey', @Text, 110, 5)

SET @Text = 'В дополнение к ответу @Kyubey, давайте рассмотрим вопрос с другой стороны.' + @NewLine +
			'Если у вас в процессе выполнения программы случился NullReferenceException при доступе по какой - то ссылке, вы должны прежде всего задать себе важный вопрос:' + @NewLine +
			'а имеет ли право эта ссылка иметь значение null ?' + @NewLine +
			'Во многих случаях правильным ответом будет «нет», и значит, исправлять придётся истинную причину ошибки, которая находится в другом месте, и произошла раньше.'
				
INSERT INTO [answer] ([question_id], [create_date], [author_name], [answer_text], [vote_positive], [vote_negative])
VALUES (@QuestionId, '20151126 13:09:00', 'VladD', @Text, 40, 2)

SET @Text = 'Вопросы о литературе по различным языкам программирования возникают очень часто. Здесь мы попробуем собрать лучшие ответы и рекомендации насчёт литературы и других учебных ресурсов по языку C#, платформе и популярным библиотекам.' + @NewLine +
			'Не забывайте, однако, что никакая теория не заменит опыта программирования! Читайте, пробуйте, тренируйтесь. Спрашивайте, если непонятно. Попробуйте запрограммировать свой проект, это лучший путь.'
				
INSERT INTO [question] ([create_date], [author_name], [topic], [question_text])
VALUES ('20180406 15:07:00', 'Nofate', 'Книги и учебные ресурсы по C#', @Text)

SET @QuestionId = (SELECT MAX([question_id]) FROM [question]);

INSERT INTO [tag_in_question] ([question_id], [tag_name])
VALUES	(@QuestionId, 'c#'),
		(@QuestionId, 'книги'),
		(@QuestionId, 'faq')

SET @Text = 'Книги для новичков: а о чём это вообще?' + @NewLine +
			@NewLine +
			'Head First C#, Jennifer Greene, Andrew Stellman (русский перевод: Изучаем C#, Д. Грин, Э. Стиллмен). Содержит упражнения. Рекомендуется многими как хорошая книга для новичков.' + @NewLine +
			@NewLine +
			'C# 6.0 and the .NET 4.6 Framework (7th Edition), Andrew Troelsen, Philip Japikse (русский перевод предыдущего издания: Язык программирования C# 5.0 и платформа .NET 4.5, Эндрю Троелсен). Хорошая популярная книга, многие начинали с неё.' + @NewLine +
			@NewLine +
			'Книги среднего уровня: если hello world не проблема' + @NewLine +
			@NewLine +
			'CLR via C#. Программирование на платформе Microsoft .NET Framework 4.5 на языке C#, Джеффри Рихтер. Неувядающая классика. Хотите знать, что и как происходит на самом деле? Это книжка для вас. Не самое живое изложение, местами скучновата, зато максимум подробностей из первых рук.' + @NewLine +
			'Предупреждение: Русский перевод от «Питер» ужасен: вас ждут выброшенные абзацы, опечатки и ляпы, меняющие смысл текста на противоположный. По возможности, читайте английский оригинал.' + @NewLine +
			@NewLine +
			'Effective C# и More Effective C#, Bill Wagner. О том, как надо и как не надо программировать на C#. Разбираются отдельные аспекты программирования, способствует углублению понимания языка.'

				
INSERT INTO [answer] ([question_id], [create_date], [author_name], [answer_text], [vote_positive], [vote_negative])
VALUES (@QuestionId, '20201101 16:48:00', 'VladD', @Text, 196, 0)

SET @Text = 'Необходимо извлечь все URL из атрибутов href тегов a в HTML странице. Я попробовал воспользоваться регулярными выражениями.' + @NewLine +
			@NewLine +
			'Но возникает множество потенциальных проблем:' + @NewLine +
			@NewLine +
			'Как отфильтровать только специфические ссылки, например, по CSS классу?' + @NewLine +
			'Что будет, если кавычки у атрибута другие?' + @NewLine +
			'Что будет, если вокруг знака равенства пробелы?' + @NewLine +
			'Что будет, если кусок страницы закомментирован?' + @NewLine +
			'Что будет, если попадётся кусок JavaScript?' + @NewLine +
			'И так далее.' + @NewLine +
			@NewLine +
			'Регулярное выражение очень быстро становится монструозным и нечитаемыми, а проблемных мест обнаруживается всё больше и больше.' + @NewLine +
			@NewLine +
			'Что делать?'

				
INSERT INTO [question] ([create_date], [author_name], [topic], [question_text])
VALUES ('20150501 01:32:00', 'Kyubey', 'Как распарсить HTML в .NET?', @Text)

SET @QuestionId = (SELECT MAX([question_id]) FROM [question]);

INSERT INTO [tag_in_question] ([question_id], [tag_name])
VALUES	(@QuestionId, 'c#'),
		(@QuestionId, 'книги'),
		(@QuestionId, 'faq')

SET @Text = 'Для парсинга HTML используте AngleSharp.' + @NewLine +
			@NewLine +
			'Если вам нужно не только распарсить HTML, но и запустить полноценный браузер, выполнить все скрипты, понажимать на кнопки и посмотреть, что получилось, то используйте CefSharp или Selenium. Учтите, что это будет на порядки медленнее.'
				
INSERT INTO [answer] ([question_id], [create_date], [author_name], [answer_text], [vote_positive], [vote_negative])
VALUES (@QuestionId, '20150501 09:32:00', 'Kyubey', @Text, 128, 2)

SET @Text = 'Используйте библиотеку CefSharp для решения подобных задач.'
				
INSERT INTO [answer] ([question_id], [create_date], [author_name], [answer_text], [vote_positive], [vote_negative])
VALUES (@QuestionId, '20161127 07:31:00', 'Vadim Ovchinnikov', @Text, 26, 2)

SET @Text = 'Если требования к производительности не очень высокие, можно использовать COM-объект Internet Explorer (добавить ссылку на Microsoft HTML Object Library).'
				
INSERT INTO [answer] ([question_id], [create_date], [author_name], [answer_text], [vote_positive], [vote_negative])
VALUES (@QuestionId, '20171101 06:14:00', 'MSDN.WhiteKnight', @Text, 8, 0)

SET @Text = 'Поиск на странице всех ссылок на книги по F#.'
				
INSERT INTO [answer] ([question_id], [create_date], [author_name], [answer_text], [vote_positive], [vote_negative])
VALUES (@QuestionId, '20190210 04:32:00', 'Anatol', @Text, 5, 3)


SET @Text = 'У меня все замечательно получается при помощи XElement Попробуйте :)' + @NewLine +
			'Как подсказали в комментариях, это будет работать если нужная нам страница является валидным XHTML документом.'
				
INSERT INTO [answer] ([question_id], [create_date], [author_name], [answer_text], [vote_positive], [vote_negative])
VALUES (@QuestionId, '20150914 05:31:00', 'iRumba', @Text, 8, 12)
GO

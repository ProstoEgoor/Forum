using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.ModelWrapper;

namespace ForumConsole.Mocks {
    public static class MocksFabric {
        public static IEnumerable<Question> MockQuestion() {
            List<Question> questions = new List<Question>();
            Question question1 = new Question(new string[] { "c#", ".net", "vb.net", "faq", "c#-faq" }) {
                Author = "Kyubey",
                CreateDate = new DateTime(2015, 4, 1, 10, 18, 0),
                Topic = "Что такое NullReferenceException, и как мне исправить код?",
                Text = @"Когда я выполняю некоторый код, выбрасывается исключение NullReferenceException со следующим сообщением:
Object reference not set to an instance of an object.
или
В экземпляре объекта не задана ссылка на объект.
Что это значит, и как мне исправить код ?"
            };
            question1.AddAnswer(new Answer(105) {
                Author = "Kyubey",
                CreateDate = new DateTime(2015, 4, 1, 10, 18, 0),
                Text = @"Вы пытаетесь воспользоваться чем-то, что равно null (или Nothing в VB.NET). Это означает, что либо вы присвоили это значение, либо вы ничего не присваивали.

Как и любое другое значение, null может передаваться от объекта к объекту, от метода к методу. Если нечто равно null в методе ""А"", вполне может быть, что метод ""В"" передал это значение в метод ""А""."
            });
            question1.AddAnswer(new Answer(38) {
                Author = "VladD",
                CreateDate = new DateTime(2015, 11, 26, 13, 9, 0),
                Text = @"В дополнение к ответу @Kyubey, давайте рассмотрим вопрос с другой стороны.
Если у вас в процессе выполнения программы случился NullReferenceException при доступе по какой - то ссылке, вы должны прежде всего задать себе важный вопрос:
а имеет ли право эта ссылка иметь значение null ?
Во многих случаях правильным ответом будет «нет», и значит, исправлять придётся истинную причину ошибки, которая находится в другом месте, и произошла раньше."
            });
            questions.Add(question1);

            Question question2 = new Question(new string[] { "c#", "книги", "faq"}) {
                Author = "Nofate",
                CreateDate = new DateTime(2018, 4, 6, 15, 7, 0),
                Topic = "Книги и учебные ресурсы по C#",
                Text = @"Вопросы о литературе по различным языкам программирования возникают очень часто. Здесь мы попробуем собрать лучшие ответы и рекомендации насчёт литературы и других учебных ресурсов по языку C#, платформе и популярным библиотекам.
Не забывайте, однако, что никакая теория не заменит опыта программирования! Читайте, пробуйте, тренируйтесь. Спрашивайте, если непонятно. Попробуйте запрограммировать свой проект, это лучший путь."
            };

            question2.AddAnswer(new Answer(196) {
                Author = "VladD",
                CreateDate = new DateTime(2020, 11, 9, 16, 48, 0),
                Text = @"Книги для новичков: а о чём это вообще?

Head First C#, Jennifer Greene, Andrew Stellman (русский перевод: Изучаем C#, Д. Грин, Э. Стиллмен). Содержит упражнения. Рекомендуется многими как хорошая книга для новичков.

C# 6.0 and the .NET 4.6 Framework (7th Edition), Andrew Troelsen, Philip Japikse (русский перевод предыдущего издания: Язык программирования C# 5.0 и платформа .NET 4.5, Эндрю Троелсен). Хорошая популярная книга, многие начинали с неё.

Книги среднего уровня: если hello world не проблема

CLR via C#. Программирование на платформе Microsoft .NET Framework 4.5 на языке C#, Джеффри Рихтер. Неувядающая классика. Хотите знать, что и как происходит на самом деле? Это книжка для вас. Не самое живое изложение, местами скучновата, зато максимум подробностей из первых рук.
Предупреждение: Русский перевод от «Питер» ужасен: вас ждут выброшенные абзацы, опечатки и ляпы, меняющие смысл текста на противоположный. По возможности, читайте английский оригинал.

Effective C# и More Effective C#, Bill Wagner. О том, как надо и как не надо программировать на C#. Разбираются отдельные аспекты программирования, способствует углублению понимания языка."
            });

            questions.Add(question2);

            Question question3 = new Question(new string[] { "c#", "книги", "faq" }) {
                Author = "Kyubey",
                CreateDate = new DateTime(2015, 5, 1, 9, 32, 0),
                Topic = "Как распарсить HTML в .NET?",
                Text = @"Необходимо извлечь все URL из атрибутов href тегов a в HTML странице. Я попробовал воспользоваться регулярными выражениями.

Но возникает множество потенциальных проблем:

Как отфильтровать только специфические ссылки, например, по CSS классу?
Что будет, если кавычки у атрибута другие?
Что будет, если вокруг знака равенства пробелы?
Что будет, если кусок страницы закомментирован?
Что будет, если попадётся кусок JavaScript?
И так далее.

Регулярное выражение очень быстро становится монструозным и нечитаемыми, а проблемных мест обнаруживается всё больше и больше.

Что делать?"
            };

            question3.AddAnswer(new Answer(126) {
                Author = "Kyubey",
                CreateDate = new DateTime(2015, 5, 1, 9, 32, 0),
                Text = @"Для парсинга HTML используте AngleSharp.

Если вам нужно не только распарсить HTML, но и запустить полноценный браузер, выполнить все скрипты, понажимать на кнопки и посмотреть, что получилось, то используйте CefSharp или Selenium. Учтите, что это будет на порядки медленнее."
            });
            question3.AddAnswer(new Answer(23) {
                Author = "VadimOvchinnikov",
                CreateDate = new DateTime(2016, 11, 27, 7, 31, 0),
                Text = @"Используйте библиотеку CefSharp для решения подобных задач."
            });
            question3.AddAnswer(new Answer(8) {
                Author = "MSDN.WhiteKnight",
                CreateDate = new DateTime(2017, 11, 1, 6, 14, 0),
                Text = @"Если требования к производительности не очень высокие, можно использовать COM-объект Internet Explorer (добавить ссылку на Microsoft HTML Object Library)."
            });
            question3.AddAnswer(new Answer(2) {
                Author = "Anatol",
                CreateDate = new DateTime(2019, 2, 10, 4, 32, 0),
                Text = @"Поиск на странице всех ссылок на книги по F#."
            });
            question3.AddAnswer(new Answer(-4) {
                Author = "iRumba",
                CreateDate = new DateTime(2015, 9, 14, 5, 31, 0),
                Text = @"У меня все замечательно получается при помощи XElement Попробуйте :)

Как подсказали в комментариях, это будет работать если нужная нам страница является валидным XHTML документом."
            });

            questions.Add(question3);

            return questions;
        }
    }
}

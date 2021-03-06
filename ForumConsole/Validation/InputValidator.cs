using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.Validation {
    static class InputValidator {
        public static string ReadAuthorQ() {
            Console.Write("Введите автора вопроса: ");
            return Console.ReadLine();
        }

        public static string ReadAuthorA() {
            Console.Write("Введите автора ответа: ");
            return Console.ReadLine();
        }

        public static string[] ReadTags() {
            Console.WriteLine("Введите теги через пробел: ");
            return Console.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        public static string ReadTopic() {
            Console.Write("Введите тему вопроса: ");
            return Console.ReadLine();
        }

        public static string ReadQuestionMsg() {
            Console.WriteLine("Введите текст вопроса, чтобы закончить ввод нажмите ctrl + enter: ");
            return ReadAreaText();
        }

        public static string ReadAnswerMsg() {
            Console.WriteLine("Введите текст ответа, чтобы закончить ввод нажмите ctrl + enter: ");
            return ReadAreaText();
        }

        public static int ReadRating() {
            Console.Write("Введите рейтинг ответа: ");
            int value;
            while (!int.TryParse(Console.ReadLine(), out value)) {
                Console.Error.WriteLine("Неверный ввод!");
                Console.Write("Введите рейтинг ответа: ");
            }
            return value;
        }

        private static string ReadAreaText() {
            int numOfRow = 1;
            string answer = "";
            string buffer = "";

            ConsoleKeyInfo pressedKey = new ConsoleKeyInfo();
            while (pressedKey.Key != ConsoleKey.Enter || pressedKey.Modifiers != ConsoleModifiers.Control) {
                pressedKey = Console.ReadKey(true);

                if (Char.IsLetterOrDigit(pressedKey.KeyChar)
                    || Char.IsSeparator(pressedKey.KeyChar)
                    || Char.IsPunctuation(pressedKey.KeyChar)) {
                    buffer += pressedKey.KeyChar;
                    Console.Write(pressedKey.KeyChar);
                } else if (pressedKey.Key == ConsoleKey.Backspace) {
                    if (WriteBackspace(buffer.Length)) {
                        buffer = buffer.Substring(0, buffer.Length - 1);
                    } else if (numOfRow > 1) {
                        --numOfRow;
                        int startIndex = answer.LastIndexOf(Environment.NewLine);
                        if (startIndex == -1) {
                            buffer = answer;
                            answer = "";
                        } else {
                            buffer = answer.Substring(startIndex + Environment.NewLine.Length);
                            answer = answer.Substring(0, startIndex);
                        }
                        JumpToPreLine(buffer.Length);
                    }
                } else if (pressedKey.Key == ConsoleKey.Enter || pressedKey.Key == ConsoleKey.Escape) {
                    if (answer.Length > 0) {
                        answer += Environment.NewLine;
                    }
                    answer += buffer;
                    buffer = "";
                    ++numOfRow;
                    Console.Write(Environment.NewLine);
                }
            }

            return answer;
        }

        private static bool WriteBackspace(int length) {
            int currentRowCursor = Console.CursorLeft;

            if (length <= 0) {
                return false;
            }

            if (currentRowCursor == 0) {
                JumpToPreLine(length);
                currentRowCursor = Console.CursorLeft;
            } else {
                --currentRowCursor;
            }

            Console.SetCursorPosition(currentRowCursor, Console.CursorTop);
            Console.Write(' ');
            Console.SetCursorPosition(currentRowCursor, Console.CursorTop);


            return true;
        }

        private static void JumpToPreLine(int length) {
            int currentLineCursor = Console.CursorTop;

            if (currentLineCursor == 0) {
                return;
            }

            --currentLineCursor;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            int targetRowCursor = (length % Console.WindowWidth == 0) ? Math.Min(Console.WindowWidth - 1, length) : length % Console.WindowWidth;
            Console.SetCursorPosition(targetRowCursor, currentLineCursor);
        }
    }
}

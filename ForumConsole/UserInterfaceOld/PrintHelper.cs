using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public static class PrintHelper {
        public static bool TryGetLine(string text, int width, ref int start, out string line) {
            if (width <= 1) {
                throw new ArgumentException("Width must be larger than 1.");
            }

            if (start >= text.Length) {
                line = "";
                return false;
            }

            if (start != -1) {
                if (text[start] == '\r') {
                    start++;
                }

                if (text[start] == '\n') {
                    start++;
                }
            } else {
                start = 0;
            }

            if (start >= text.Length) {
                line = "";
                return true;
            }

            int end = text.IndexOfAny(new char[] { '\n', '\r' }, start, Math.Min(width, text.Length - start));

            if (start + width >= text.Length) {
                if (end == -1) {
                    end = text.Length;
                }
            } else {
                if (end == -1) {
                    end = text.LastIndexOf(' ', start + width, width);
                }
                if (end == -1) {
                    end = start + width;
                }
            }

            if (start == end) {
                line = "";
            } else {
                line = text[start..end];
            }

            start = end;

            return true;
        }

        public static string GetNumAddition(int num, string first, string second, string third) {
            int preLastDigit = num % 100 / 10;

            if (preLastDigit == 1) {
                return third;
            }

            switch (num % 10) {
                case 1:
                    return first;
                case 2:
                case 3:
                case 4:
                    return second;
                default:
                    return third;
            }

        }
    }
}

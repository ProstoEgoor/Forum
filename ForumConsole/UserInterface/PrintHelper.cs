using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    public static class PrintHelper {
        public static bool TryGetLineBoundaries(string text, int start, int width, out (int start, int end) boundaries) {
            if (width <= 1) {
                throw new ArgumentException("Width must be larger than 1.");
            }
            while (start < text.Length && (text[start] == '\n' || text[start] == '\r' || text[start] == ' ')) {
                ++start;
            }
            if (start >= text.Length) {
                boundaries = (start, start);
                return false;
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

            boundaries = (start, end);
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

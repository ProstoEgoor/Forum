using System;
using System.Collections.Generic;
using System.Text;

namespace ForumConsole.UserInterface {
    /*static class SelectFile {
        public static void SaveToFile<T>(IEnumerable<T> data, string title) {
            Console.Clear();
            Console.WriteLine($"{title}: сохранение в файл");
            string fileName = FileValidator.ReadPathToSave();
            FileTypes fileType = FileValidator.ReadFileType();
            try {
                switch (fileType) {
                    case FileTypes.Xml: FileManager.SaveToXml(fileName, data.ToArray()); break;
                    case FileTypes.Json: FileManager.SaveToJson(fileName, data); break;
                }
                Console.WriteLine("Файл успешно сохранен!");
            } catch (Exception e) {
                Console.WriteLine("При сохранении файла произошла ошибка: " + e.Message);
            }
        }


        public static IEnumerable<T> LoadFromFile<T>(string title) {
            Console.Clear();
            Console.WriteLine($"{title}: загрузка из файла");
            Console.WriteLine("Внимание! Все существующие данные будут удалены и перезаписаны из файла. Продолжить ? (да/нет)");
            if (!FileValidator.ReadYesNo()) {
                return null;
            }
            string fileName = FileValidator.ReadPathToLoad();
            FileTypes? fileType = FileManager.CheckFileType(fileName);
            IEnumerable<T> data = null;
            try {
                switch (fileType) {
                    case FileTypes.Xml: data = FileManager.LoadFromXml<T>(fileName); break;
                    case FileTypes.Json: data = FileManager.LoadFromJson<T>(fileName); break;
                    default: throw new InvalidOperationException("Формат файла не распознан. Используйте XML или JSON.");
                }
                Console.WriteLine("Файл успешно загружен!");
            } catch (Exception e) {
                Console.WriteLine("При загрузке файла произошла ошибка: " + e.Message);
            }
            return data;
        }
    }*/
}

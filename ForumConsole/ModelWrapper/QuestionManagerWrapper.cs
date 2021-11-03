using System;
using System.Collections.Generic;
using System.Text;
using ForumModel;
using ForumConsole.UserInterface;
using System.Linq;
using ForumConsole.FIles;

namespace ForumConsole.ModelWrapper {
    public class QuestionManagerWrapper : IConsoleEditableContainer<Question>, IFileEditable {
        public QuestionManager QuestionManager { get; private set; }

        public bool Find { get; set; }
        public string FindText { get; set; } = "";
        public List<string> FindTags { get; } = new List<string>();

        public QuestionManagerWrapper(QuestionManager questionManager) {
            QuestionManager = questionManager;
        }

        public IReadOnlyList<QuestionWrapper> GetWrappedQuestions() {
            if (Find) {
                return QuestionManager.GetFilteredQuestions(FindText, FindTags).Select(question => new QuestionWrapper(question)).ToList();
            } else {
                return QuestionManager.Questions.Select(question => new QuestionWrapper(question)).ToList();
            }
        }

        public void Add(Question item) {
            QuestionManager.AddQuestion(item);
        }

        public bool Remove(Question item) {
            return QuestionManager.RemoveQuestion(item);
        }

        public bool Replace(Question oldItem, Question newItem) {
            newItem.AddAnswer(oldItem.Answers.ToArray());
            return QuestionManager.ReplaceQuestion(oldItem, newItem);
        }

        public bool Save(string path, out string error) {
            error = "";
            FileTypes? fileType = FileManager.CheckFileType(path);
            try {
                switch (fileType) {
                    case FileTypes.Xml: FileManager.SaveToXml(path, QuestionManager.Questions.Select(question => QuestionFileDto.Map(question)).ToArray()); break;
                    case FileTypes.Json: FileManager.SaveToJson(path, QuestionManager.Questions.Select(question => QuestionFileDto.Map(question)).ToArray()); break;
                    default: error = "не введено расширение"; return false;
                }

                return true;
            } catch (Exception e) {
                error = e.Message;
                return false;
            }
        }

        public bool Load(string path, out string error) {
            error = "";
            FileTypes? fileType = FileManager.CheckFileType(path);
            IEnumerable<Question> questions = null;
            try {
                switch (fileType) {
                    case FileTypes.Xml: questions = FileManager.LoadFromXml<QuestionFileDto>(path).Select(question => QuestionFileDto.Map(question)); break;
                    case FileTypes.Json: questions = FileManager.LoadFromJson<QuestionFileDto>(path).Select(question => QuestionFileDto.Map(question)); break;
                    default: error = "Формат файла не распознан. Используйте XML или JSON."; return false;
                }

                TagManager tagManager = new TagManager();
                QuestionManager newQuestionManager = new QuestionManager(tagManager, questions);

                QuestionManager = newQuestionManager;

                return true;
            } catch (Exception e) {
                error = e.Message;
                return false;
            }
        }
    }
}

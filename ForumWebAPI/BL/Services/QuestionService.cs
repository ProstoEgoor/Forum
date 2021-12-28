using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Repositories;
using ForumWebAPI.BL.Model;

namespace ForumWebAPI.BL.Services {
    public class QuestionService {

        QuestionRepository QuestionRepository { get; }
        AnswerRepository AnswerRepository { get; }
        public QuestionService(QuestionRepository questionRepository, AnswerRepository answerRepository) {
            QuestionRepository = questionRepository;
            AnswerRepository = answerRepository;
        }
        public async Task<(QuestionDetailApiDto, Exception)> GetAsync(int questionId, bool? dateSort, bool ratingSort) {
            var question = await QuestionRepository.GetAsync(questionId, dateSort, ratingSort);
            if (question == null) {
                return (null, new KeyNotFoundException($"Вопрос с id:{questionId} не найден."));
            } else {
                return (new QuestionDetailApiDto(question), null);
            }
        }

        public IAsyncEnumerable<QuestionApiDto> GetAllAsync(string textSearch, IEnumerable<string> tagsFilter) {
            var questions = QuestionRepository.GetAllAsync(textSearch, tagsFilter);
            return questions.Select(question => new QuestionApiDto(question));
        }


        public async Task<(QuestionApiDto, Exception)> CreateAsync(QuestionCreateApiDto question) {

            var questionToCreate = question.Create("author");
            QuestionRepository.Create(questionToCreate);

            try {
                await QuestionRepository.SaveAsync();
            } catch (Exception e) {
                return (null, e);
            }

            return (new QuestionApiDto(questionToCreate), null);
        }

        public async Task<Exception> UpdateAsync(int questionId, QuestionEditApiDto question) {
            var questionToUpdate = await QuestionRepository.GetAsync(questionId);

            if (questionToUpdate == null) {
                return new KeyNotFoundException($"Вопрос с id:{questionId} не найден.");
            }

            question.Update(questionToUpdate);
            QuestionRepository.Update(questionToUpdate);

            try {
                await QuestionRepository.SaveAsync();
            } catch (Exception e) {
                return e;
            }

            return null;
        }

        public async Task<(QuestionDetailApiDto, Exception)> DeleteAsync(int questionId) {
            var questionToDelete = await QuestionRepository.DeleteAsync(questionId);

            if (questionToDelete == null) {
                return (null, new KeyNotFoundException($"Вопрос с id:{questionId} не найден."));
            }

            try {
                await QuestionRepository.SaveAsync();
            } catch (Exception e) {
                return (null, e);
            }

            return (new QuestionDetailApiDto(questionToDelete), null);
        }
    }
}

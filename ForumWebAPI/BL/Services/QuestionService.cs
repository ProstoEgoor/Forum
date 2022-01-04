using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Repositories;
using ForumWebAPI.BL.Model;
using ForumWebAPI.BL.Exceptions;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Services {
    public class QuestionService {
        private readonly QuestionRepository questionRepository;
        public QuestionService(QuestionRepository questionRepository, AnswerRepository answerRepository) {
            this.questionRepository = questionRepository;
        }
        public async Task<(QuestionDetailApiDto, Exception)> GetAsync(long questionId, bool? dateSort, bool ratingSort) {
            var question = await questionRepository.GetAsync(questionId, dateSort, ratingSort);
            if (question == null) {
                return (null, new KeyNotFoundException($"Вопрос с id:{questionId} не найден."));
            } else {
                return (new QuestionDetailApiDto(question), null);
            }
        }

        public IAsyncEnumerable<QuestionApiDto> GetAllAsync(string textSearch, IEnumerable<string> tagsFilter) {
            var questions = questionRepository.GetAllAsync(textSearch, tagsFilter);
            return questions.Select(question => new QuestionApiDto(question));
        }


        public async Task<(QuestionApiDto, Exception)> CreateAsync(QuestionCreateApiDto question, string authorId) {

            var questionToCreate = question.Create(authorId);
            questionRepository.Create(questionToCreate);

            try {
                await questionRepository.SaveAsync();
                await questionRepository.LoadAuthor(questionToCreate);
            } catch (Exception e) {
                return (null, new SaveChangesException(e));
            }

            return (new QuestionApiDto(questionToCreate), null);
        }

        public async Task<Exception> UpdateAsync(long questionId, QuestionEditApiDto question) {
            var questionToUpdate = await questionRepository.GetAsync(questionId);

            if (questionToUpdate == null) {
                return new KeyNotFoundException($"Вопрос с id:{questionId} не найден.");
            }

            return await UpdateAsync(questionToUpdate, question);
        }

        public async Task<Exception> UpdateAsync(QuestionDbDTO questionToUpdate, QuestionEditApiDto question) {
            if (questionToUpdate == null) {
                return new ArgumentNullException();
            }

            question.Update(questionToUpdate);
            questionRepository.Update(questionToUpdate);

            try {
                await questionRepository.SaveAsync();
            } catch (Exception e) {
                return new SaveChangesException(e);
            }

            return null;
        }


        public async Task<(QuestionDetailApiDto, Exception)> DeleteAsync(long questionId) {
            var questionToDelete = await questionRepository.DeleteAsync(questionId);

            if (questionToDelete == null) {
                return (null, new KeyNotFoundException($"Вопрос с id:{questionId} не найден."));
            }

            return await DeleteAsync(questionToDelete);

        }

        public async Task<(QuestionDetailApiDto, Exception)> DeleteAsync(QuestionDbDTO questionToDelete) {
            if (questionToDelete == null) {
                return (null, new ArgumentNullException());
            }

            try {
                await questionRepository.SaveAsync();
            } catch (Exception e) {
                return (null, new SaveChangesException(e));
            }

            return (new QuestionDetailApiDto(questionToDelete), null);
        }
    }
}

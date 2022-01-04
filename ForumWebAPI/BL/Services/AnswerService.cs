using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Repositories;
using ForumWebAPI.BL.Model;
using ForumWebAPI.BL.Exceptions;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Services {
    public class AnswerService {
        private readonly QuestionRepository questionRepository;
        private readonly AnswerRepository answerRepository;
        public AnswerService(QuestionRepository questionRepository, AnswerRepository answerRepository) {
            this.questionRepository = questionRepository;
            this.answerRepository = answerRepository;
        }

        public async Task<(AnswerApiDto, Exception)> GetAsync(long answerId) {
            var answer = await answerRepository.GetAsync(answerId);

            if (answer == null) {
                return (null, new KeyNotFoundException($"Ответ с id:{answerId} не найден."));
            } else {
                return (new AnswerApiDto(answer), null);
            }
        }

        public async Task<(IAsyncEnumerable<AnswerApiDto>, Exception)> GetAssociatedAsync(long questionId, bool? dateSort, bool ratingSort) {
            if (!await questionRepository.ExistAsync(questionId)) {
                return (null, new KeyNotFoundException($"Вопрос с id:{questionId} не найден."));
            }

            var answers = answerRepository.GetAssociatedAsync(questionId, dateSort, ratingSort);

            return (answers.Select(answer => new AnswerApiDto(answer)), null);
        }

        public async Task<(AnswerApiDto, Exception)> CreateAsync(AnswerCreateApiDto answer, string authorId) {
            if (!await questionRepository.ExistAsync(answer.QuestionId)) {
                return (null, new KeyNotFoundException($"Вопрос с id:{answer.QuestionId} не найден."));
            }

            var answerToCreate = answer.Create(authorId);
            answerRepository.Create(answerToCreate);

            try {
                await answerRepository.SaveAsync();
                await answerRepository.LoadAuthor(answerToCreate);
            } catch (Exception e) {
                return (null, new SaveChangesException(e));
            }


            return (new AnswerApiDto(answerToCreate), null);
        }

        public async Task<Exception> UpdateAsync(long answerId, AnswerEditApiDto answer) {
            var answerToUpdate = await answerRepository.GetAsync(answerId);

            if (answerToUpdate == null) {
                return new KeyNotFoundException($"Вопрос с id:{answerId} не найден.");
            }

            return await UpdateAsync(answerToUpdate, answer);
        }

        public async Task<Exception> UpdateAsync(AnswerDbDTO answerToUpdate, AnswerEditApiDto answer) {
            if (answerToUpdate == null) {
                return new ArgumentNullException();
            }

            answer.Update(answerToUpdate);
            answerRepository.Update(answerToUpdate);

            try {
                await answerRepository.SaveAsync();
            } catch (Exception e) {
                return new SaveChangesException(e);
            }

            return null;
        }

        public async Task<(AnswerApiDto, Exception)> DeleteAsync(long answerId) {
            var answerToDelete = await answerRepository.DeleteAsync(answerId);

            if (answerToDelete == null) {
                return (null, new ArgumentNullException($"Ответ с id:{answerId} не найден."));
            }

            return await DeleteAsync(answerToDelete);
        }

        public async Task<(AnswerApiDto, Exception)> DeleteAsync(AnswerDbDTO answerToDelete) {
            if (answerToDelete == null) {
                return (null, new ArgumentNullException());
            }

            try {
                await answerRepository.SaveAsync();
            } catch (Exception e) {
                return (null, new SaveChangesException(e));
            }

            return (new AnswerApiDto(answerToDelete), null);
        }

        public async Task<Exception> VoteAsync(long answerId, bool positiveVote) {
            var answer = await answerRepository.GetAsync(answerId);

            if (answer == null) {
                return new ArgumentNullException($"Ответ с id:{answerId} не найден.");
            }

            if (positiveVote) {
                answer.VotePositive++;
            } else {
                answer.VoteNegative++;
            }

            answerRepository.Update(answer);

            try {
                await answerRepository.SaveAsync();
            } catch (Exception e) {
                return new SaveChangesException(e);
            }

            return null;
        }
    }
}

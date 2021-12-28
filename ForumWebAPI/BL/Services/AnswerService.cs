using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Repositories;
using ForumWebAPI.BL.Model;

namespace ForumWebAPI.BL.Services {
    public class AnswerService {
        QuestionRepository QuestionRepository { get; }
        AnswerRepository AnswerRepository { get; }
        public AnswerService(QuestionRepository questionRepository, AnswerRepository answerRepository) {
            QuestionRepository = questionRepository;
            AnswerRepository = answerRepository;
        }

        public async Task<(AnswerApiDto, Exception)> GetAsync(int answerId) {
            var answer = await AnswerRepository.GetAsync(answerId);

            if (answer == null) {
                return (null, new KeyNotFoundException($"Ответ с id:{answerId} не найден."));
            } else {
                return (new AnswerApiDto(answer), null);
            }
        }

        public async Task<(IAsyncEnumerable<AnswerApiDto>, Exception)> GetAssociatedAsync(int questionId, bool? dateSort, bool ratingSort) {
            if (!await QuestionRepository.ExistAsync(questionId)) {
                return (null, new KeyNotFoundException($"Вопрос с id:{questionId} не найден."));
            }

            var answers = AnswerRepository.GetAssociatedAsync(questionId, dateSort, ratingSort);

            return (answers.Select(answer => new AnswerApiDto(answer)), null);
        }

        public async Task<(AnswerApiDto, Exception)> CreateAsync(AnswerCreateApiDto answer) {
            if (!await QuestionRepository.ExistAsync(answer.QuestionId)) {
                return (null, new KeyNotFoundException($"Вопрос с id:{answer.QuestionId} не найден."));
            }

            var answerToCreate = answer.Create("author");
            AnswerRepository.Create(answerToCreate);

            try {
                await AnswerRepository.SaveAsync();
            } catch (Exception e) {
                return (null, e);
            }


            return (new AnswerApiDto(answerToCreate), null);
        }

        public async Task<Exception> UpdateAsync(int answerId, AnswerEditApiDto answer) {
            var answerToUpdate = await AnswerRepository.GetAsync(answerId);

            if (answerToUpdate == null) {
                return new KeyNotFoundException($"Вопрос с id:{answerId} не найден.");
            }

            answer.Update(answerToUpdate);
            AnswerRepository.Update(answerToUpdate);

            try {
                await AnswerRepository.SaveAsync();
            } catch (Exception e) {
                return e;
            }

            return null;
        }

        public async Task<(AnswerApiDto, Exception)> DeleteAsync(int answerId) {
            var answerToDelete = await AnswerRepository.DeleteAsync(answerId);

            if (answerToDelete == null) {
                return (null, new KeyNotFoundException($"Вопрос с id:{answerId} не найден."));
            }

            try {
                await AnswerRepository.SaveAsync();
            } catch (Exception e) {
                return (null, e);
            }

            return (new AnswerApiDto(answerToDelete), null);
        }

        public async Task<Exception> VoteAsync(int answerId, bool positiveVote) {
            var answer = await AnswerRepository.GetAsync(answerId);

            if (answer == null) {
                return new KeyNotFoundException($"Вопрос с id:{answerId} не найден.");
            }

            if (positiveVote) {
                answer.VotePositive++;
            } else {
                answer.VoteNegative++;
            }

            AnswerRepository.Update(answer);

            try {
                await AnswerRepository.SaveAsync();
            } catch (Exception e) {
                return e;
            }

            return null;
        }
    }
}

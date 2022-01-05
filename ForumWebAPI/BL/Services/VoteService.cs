using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumDbContext.Repositories;
using ForumWebAPI.BL.Model;
using ForumWebAPI.BL.Exceptions;
using ForumDbContext.Model.DTO;

namespace ForumWebAPI.BL.Services {
    public class VoteService {
        private readonly VoteRepository voteRepository;
        private readonly AnswerRepository answerRepository;

        public VoteService(VoteRepository voteRepository, AnswerRepository answerRepository) {
            this.voteRepository = voteRepository;
            this.answerRepository = answerRepository;
        }

        public async Task<AnswerApiDto> IncludeVoteAsync(AnswerApiDto answer, string authorId) {
            answer.MyVote = await voteRepository.GetVoteResultAsync(answer.Id, authorId);
            return answer;
        }

        public async IAsyncEnumerable<AnswerApiDto> IncludeVoteAsync(IAsyncEnumerable<AnswerApiDto> answers, string authorId) {
            await foreach (var answer in answers) {
                answer.MyVote = await voteRepository.GetVoteResultAsync(answer.Id, authorId);
                yield return answer;
            }
        }

        public async Task<Exception> VoteAsync(long answerId, bool? vote, string authorId) {
            var answer = await answerRepository.GetAsync(answerId);

            if (answer == null) {
                return new KeyNotFoundException($"Ответ с id:{answerId} не найден.");
            }

            var currentVote = await voteRepository.GetAsync(answerId, authorId);

            if (currentVote == null && vote != null) {
                voteRepository.Create(new VoteDbDTO() {
                    AnswerId = answerId,
                    AuthorId = authorId,
                    Vote = (bool)vote
                });

                if ((bool)vote) {
                    answer.VotePositive++;
                } else {
                    answer.VoteNegative++;
                }

            } else if (currentVote != null && vote == null) {
                voteRepository.Delete(currentVote);

                if (currentVote.Vote) {
                    answer.VotePositive--;
                } else {
                    answer.VoteNegative--;
                }
            } else if (currentVote != null && currentVote.Vote != (bool)vote) {
                currentVote.Vote = (bool)vote;
                voteRepository.Update(currentVote);

                if ((bool)vote) {
                    answer.VoteNegative--;
                    answer.VotePositive++;
                } else {
                    answer.VotePositive--;
                    answer.VoteNegative++;
                }
            } else {
                return new AlreadyVotesException($"Ваш голос за ответ с id:{answerId} уже учтен.");
            }

            answerRepository.Update(answer);

            try {
                await answerRepository.SaveAsync();
                await voteRepository.SaveAsync();
            } catch (Exception e) {
                return new SaveChangesException(e);
            }

            return null;
        }
    }
}

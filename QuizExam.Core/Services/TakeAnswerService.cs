using QuizExam.Core.Contracts;
using QuizExam.Core.Models.TakeAnswer;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Core.Services
{
    public class TakeAnswerService : ITakeAnswerService
    {
        private readonly IApplicationDbRepository repository;

        public TakeAnswerService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> AddAnswer(string takeId, string checkedAnswerId, string questionId)
        {
            try
            {
                var result = false;
                var takeExam = await this.repository.GetByIdAsync<TakeExam>(Guid.Parse(takeId));

                if (takeId != null)
                {
                    var answer = new TakeAnswer
                    {
                        TakeExamId = Guid.Parse(takeId),
                        AnswerOptionId = Guid.Parse(checkedAnswerId),
                        QuestionId = Guid.Parse(questionId),
                    };
                    await this.repository.AddAsync(answer);
                    await this.repository.SaveChangesAsync();
                    result = true;
                }

                return result;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(TakeAnswer)}'. ");
            }
        }
    }
}

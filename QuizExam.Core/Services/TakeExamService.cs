using QuizExam.Core.Contracts;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Enums;
using QuizExam.Infrastructure.Data.Identity;
using QuizExam.Infrastructure.Data.Repositories;

namespace QuizExam.Core.Services
{
    public class TakeExamService : ITakeExamService
    {
        private readonly IApplicationDbRepository repository;

        public TakeExamService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Guid> CreateTake(string userId, string examId)
        {
            try
            {
                var isUser = await this.repository.GetByIdAsync<ApplicationUser>(userId);
                var isExam = await this.repository.GetByIdAsync<Exam>(Guid.Parse(examId));

                if (isUser != null && isExam != null)
                {
                    TakeExam newTake = new TakeExam()
                    {
                        UserId = userId,
                        ExamId = Guid.Parse(examId),
                        Status = TakeExamStatusEnum.Started
                    };

                    await this.repository.AddAsync(newTake);
                    await this.repository.SaveChangesAsync();

                    return newTake.Id;
                }

                return Guid.Empty;
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(TakeExam)}'. ");
            }
        }
    }
}

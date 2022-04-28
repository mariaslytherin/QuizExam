﻿using Microsoft.EntityFrameworkCore;
using QuizExam.Core.Contracts;
using QuizExam.Core.Models.AnswerOption;
using QuizExam.Core.Models.Exam;
using QuizExam.Core.Models.Question;
using QuizExam.Infrastructure.Data;
using QuizExam.Infrastructure.Data.Repositories;
using System.Linq;

namespace QuizExam.Core.Services
{
    public class ExamService : IExamService
    {
        private readonly IApplicationDbRepository repository;

        public ExamService(IApplicationDbRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Activate(Guid id)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(id);

            if (exam != null)
            {
                exam.IsActive = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> Create(NewExamVM model)
        {
            var exam = new Exam()
            {
                Title = model.Title,
                Description = model.Description,
                MaxScore = model.MaxScore,
                SubjectId = Guid.Parse(model.SubjectId),
            };

            await this.repository.AddAsync(exam);
            await this.repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Deactivate(Guid id)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(id);

            if (exam != null)
            {
                exam.IsActive = false;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> Delete(Guid id)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(id);

            if (exam != null)
            {
                exam.IsDeleted = true;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<bool> Edit(EditExamVM model)
        {
            bool result = false;
            var exam = await this.repository.GetByIdAsync<Exam>(Guid.Parse(model.Id));

            if (exam != null)
            {
                exam.Title = model.Title;
                exam.Description = model.Description;
                exam.MaxScore = model.MaxScore;
                exam.ModifyDate = DateTime.Today;
                await this.repository.SaveChangesAsync();
                result = true;
            }

            return result;
        }

        public async Task<ExamListVM> GetAllExams(int? page, int? size)
        {
            var exams = await this.repository.All<Exam>()
                .Where(e => !e.IsDeleted)
                .Join(this.repository.All<Subject>(),
                      e => e.SubjectId, s => s.Id,
                (e, s) => new ViewExamVM()
                {
                    Id = e.Id.ToString(),
                    Title = e.Title,
                    SubjectName = s.Name,
                    Description = e.Description,
                    CreateDate = e.CreateDate,
                    IsActive = e.IsActive ? "Да" : "Не",
                })
                .ToListAsync();

            if (size.HasValue && page.HasValue)
            {
                exams = exams
                    .OrderBy(e => e.Title)
                    .Skip((int)(page * size - size))
                    .Take((int)size).ToList();
            }

            var model = new ExamListVM()
            {
                PageNo = page,
                PageSize = size
            };

            model.TotalRecords = await this.repository.All<Exam>().Where(e => !e.IsDeleted).CountAsync();
            model.Exams = exams;

            return model;
        }

        public async Task<Exam> GetExamById(Guid id)
        {
            return await this.repository.GetByIdAsync<Exam>(id);
        }

        public async Task<EditExamVM> GetExamForEdit(Guid id)
        {
            var exam = await this.repository.GetByIdAsync<Exam>(id);
            var subject = await this.repository.GetByIdAsync<Subject>(exam.SubjectId);

            return new EditExamVM()
            {
                Id = exam.Id.ToString(),
                Title = exam.Title,
                Description = exam.Description,
                MaxScore = exam.MaxScore,
                SubjectName = subject.Name,
            };
        }

        public async Task<ViewExamVM> GetExamForView(string id)
        {
            try
            {
                var exam = await this.repository.GetByIdAsync<Exam>(Guid.Parse(id));
                var subject = await this.repository.GetByIdAsync<Subject>(exam.SubjectId);
                var hasQuestions = await this.repository.All<Question>().AnyAsync(q => q.ExamId == Guid.Parse(id) && !q.IsDeleted);

                if (hasQuestions)
                {
                    var questions = this.repository.All<Question>().Where(q => q.ExamId == Guid.Parse(id) && !q.IsDeleted)
                    .Select(q => new QuestionExamVM
                    {
                        Id = q.Id.ToString(),
                        Content = q.Content,
                        Points = q.Points,
                        AnswerOptions = this.repository
                            .All<AnswerOption>()
                            .Where(a => a.QuestionId == q.Id && !a.IsDeleted)
                            .Select(a => new AnswerOptionVM
                            {
                                Content = a.Content,
                                IsCorrect = a.IsCorrect,
                            })
                            .DefaultIfEmpty()
                            .ToList()
                    })
                    .DefaultIfEmpty()
                    .ToList();

                    return new ViewExamVM
                    {
                        Id = exam.Id.ToString(),
                        Title = exam.Title,
                        Description = exam.Description,
                        SubjectName = subject.Name,
                        Questions = questions,
                    };
                }
                else
                {
                    return new ViewExamVM
                    {
                        Id = exam.Id.ToString(),
                        Title = exam.Title,
                        Description = exam.Description,
                        SubjectName = subject.Name,
                        Questions = new List<QuestionExamVM>(),
                    };
                }
            }
            catch (NullReferenceException ex)
            {
                throw new Exception("There is no such an object.");
            }
        }
    }
}

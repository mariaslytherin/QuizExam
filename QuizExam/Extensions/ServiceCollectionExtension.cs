﻿using QuizExam.Core.Contracts;
using QuizExam.Core.Services;
using QuizExam.Infrastructure.Data.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbRepository, ApplicationDbRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAnswerOptionService, AnswerOptionService>();
            services.AddScoped<ITakeExamService, TakeExamService>();
            services.AddScoped<ITakeAnswerService, TakeAnswerService>();

            return services;
        }
    }
}

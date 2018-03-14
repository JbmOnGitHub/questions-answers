using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using ModuleZeroSampleProject.Authorization;
using ModuleZeroSampleProject.Configuration;
using ModuleZeroSampleProject.Questions.Dto;
using ModuleZeroSampleProject.Users;

namespace ModuleZeroSampleProject.Questions
{
    [AbpAuthorize]
    public class QuestionAppService : ApplicationService, IQuestionAppService
    {
        private readonly IRepository<Question> _questionRepository;
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly QuestionDomainService _questionDomainService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public QuestionAppService(IRepository<Question> questionRepository, IRepository<Answer> answerRepository, IRepository<User, long> userRepository, QuestionDomainService questionDomainService, IUnitOfWorkManager unitOfWorkManager)
        {
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _userRepository = userRepository;
            _questionDomainService = questionDomainService;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [AbpAuthorize(PermissionNames.Pages_Questions_Create)] //An example of permission checking
        public async Task Handle(CreateQuestion cmd)
        {
            await _questionRepository.InsertAsync(new Question(cmd.Title, cmd.Text));
        }
        
        public Task Handle(VoteUp cmd)
        {
            var question = _questionRepository.Get(cmd.Id);
            question.VoteCount++;
            return _questionRepository.UpdateAsync(question);
        }

        public Task Handle(VoteDown cmd)
        {
            var question = _questionRepository.Get(cmd.Id);
            question.VoteCount--;
            return _questionRepository.UpdateAsync(question);
        }

        [AbpAuthorize(PermissionNames.Pages_AnswerToQuestions)]
        public Task Handle(SubmitAnswer cmd)
        {
            var question = _questionRepository.Get(cmd.QuestionId);
            var currentUser = _userRepository.Get(AbpSession.GetUserId());

            question.AnswerCount++;

           return _answerRepository.InsertAsync(
                new Answer(cmd.Text)
                {
                    Question = question,
                    CreatorUser = currentUser
                });
        }

        public Task Handle(AcceptAnswer cmd)
        {
            var answer = _answerRepository.Get(cmd.Id);
            _questionDomainService.AcceptAnswer(answer);
            return _answerRepository.UpdateAsync(answer);
        }

        //Queries

        public PagedResultDto<QuestionDto> Query(GetQuestions input)
        {
            if (input.MaxResultCount <= 0)
            {
                input.MaxResultCount = SettingManager.GetSettingValue<int>(MySettingProvider.QuestionsDefaultPageSize);
            }

            var questionCount = _questionRepository.Count();
            var questions =
                _questionRepository
                    .GetAll()
                    .Include(q => q.CreatorUser)
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToList();

            return new PagedResultDto<QuestionDto>
            {
                TotalCount = questionCount,
                Items = questions.MapTo<List<QuestionDto>>()
            };
        }

        public GetQuestionOutput Query(GetQuestion input)
        {
            var question =
                _questionRepository
                    .GetAll()
                    .Include(q => q.CreatorUser)
                    .Include(q => q.Answers)
                    .Include("Answers.CreatorUser")
                    .FirstOrDefault(q => q.Id == input.Id);

            if (question == null)
            {
                throw new UserFriendlyException("There is no such a question. Maybe it's deleted.");
            }

            if (input.IncrementViewCount)
            {
                question.ViewCount++;
            }

            return new GetQuestionOutput
            {
                Question = question.MapTo<QuestionWithAnswersDto>()
            };
        }
    }
}
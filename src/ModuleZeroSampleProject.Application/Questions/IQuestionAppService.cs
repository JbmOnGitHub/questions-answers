using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ModuleZeroSampleProject.Questions.Dto;

namespace ModuleZeroSampleProject.Questions
{
    public interface IQuestionAppService : IApplicationService
    {
        // Commands
        Task Handle(CreateQuestion cmd);

        Task Handle(VoteUp cmd);

        Task Handle(VoteDown cmd);

        Task Handle(SubmitAnswer cmd);

        Task Handle(AcceptAnswer cmd);

        //Queries
        PagedResultDto<QuestionDto> Query(GetQuestions input);

        GetQuestionOutput Query(GetQuestion input);
    }
}

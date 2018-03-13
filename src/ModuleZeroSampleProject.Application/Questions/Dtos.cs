using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace ModuleZeroSampleProject.Questions.Dto
{
    [AutoMapFrom(typeof(Answer))]
    public class AnswerDto : CreationAuditedEntityDto
    {
        public string Text { get; set; }

        public int QuestionId { get; set; }

        public bool IsAccepted { get; set; }

        public string CreatorUserName { get; set; }
    }

    [AutoMapFrom(typeof(Question))]
    public class QuestionDto : CreationAuditedEntityDto
    {
        public string Title { get; set; }

        public string Text { get; set; }

        public int VoteCount { get; set; }

        public int AnswerCount { get; set; }

        public int ViewCount { get; set; }

        public string CreatorUserName { get; set; }
    }

    [AutoMapFrom(typeof(Question))]
    public class QuestionWithAnswersDto : QuestionDto
    {
        public List<AnswerDto> Answers { get; set; }
    }

    public class GetQuestionOutput
    {
        public QuestionWithAnswersDto Question { get; set; }
    }

    public class SubmitAnswerOutput
    {
        public AnswerDto Answer { get; set; }
    }
}
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleZeroSampleProject.Questions
{
    public class CreateQuestion : ICommand
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }
    }

    public class SubmitAnswer : ICommand
    {
        [Range(1, int.MaxValue)]
        public int QuestionId { get; set; }

        [Required]
        public string Text { get; set; }
    }

    public class VoteUp : EntityDto, ICommand
    {
        public VoteUp(int i) : base(i) { }
    }

    public class VoteDown : EntityDto, ICommand
    {
        public VoteDown(int i) : base(i) { }
    }

    public class AcceptAnswer : EntityDto, ICommand
    {
        public AcceptAnswer(int i) : base(i) { }
    }



    interface ICommand
    {

    }
}

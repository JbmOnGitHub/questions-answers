using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModuleZeroSampleProject.Questions
{
    public class GetQuestion : EntityDto, IQuery
    {
        public bool IncrementViewCount { get; set; }
    }

    public class GetQuestions : IQuery, IPagedResultRequest, ISortedResultRequest
    {
        [Range(0, 1000)]
        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }

        public string Sorting { get; set; }
    }

    interface IQuery
    {

    }
}

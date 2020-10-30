using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Task.DTOModels.ViewModels;

namespace Task.CQRS.Messages.Queries
{
    public class ListTaskQuery
    {
        public int UserId { get; set; }

        public string Description { get; set; }

        public bool? Completed { get; set; }

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        public string Sort { get; set; } = "id asc";

        public class Response
        {
            public Response(List<TaskVm> tasks, Meta meta)
            {
                Tasks = tasks;
                Meta = meta;
            }

            public List<TaskVm> Tasks { get; set; }

            public Meta Meta { get; set; }
        }
    }
}
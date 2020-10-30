using System.ComponentModel.DataAnnotations;
using Task.CQRS.Impl;
using Task.CrossCutting.Enums;
using Task.CrossCutting.Exceptions;

namespace Task.CQRS.Messages.Commands
{
    public class UpdateTaskCmd : CommandBase
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public bool? Completed { get; set; }

        public int UserId { get; set; }

        public void Validate()
        {
            if (Id != 0)
            {
                throw new CustomException((int)ExceptionCode.Validation, "Update Task mustn't have 'id'");
            }
        }

        public class Response
        {
            public Response(int id)
            {
                Id = id;
            }

            public int Id { get; set; }
        }
    }
}
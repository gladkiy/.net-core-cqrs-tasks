namespace Task.CQRS.Messages.Commands
{
    using System.ComponentModel.DataAnnotations;
    using Task.CQRS.Impl;
    using Task.CrossCutting.Enums;
    using Task.CrossCutting.Exceptions;
    using Task.DBModels.Entities;

    public class CreateTaskCmd : CommandBase
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public bool Completed { get; set; }

        public int? UserId { get; set; }

        public void Validate()
        {
            if (Id != 0)
            {
                throw new CustomException((int)ExceptionCode.Validation, "Create Task mustn't have 'id'");
            }
        }

        public Task ToTask()
        {
            return new Task
            {
                Description = Description,
                Completed = Completed,
                UserId = UserId
            };
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
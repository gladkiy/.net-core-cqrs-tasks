using Task.CQRS.Impl;

namespace Task.CQRS.Messages.Commands
{
    public class DeleteTaskCmd : CommandBase
    {
        public int Id { get; set; }
    }
}
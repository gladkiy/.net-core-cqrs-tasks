namespace Task.UnitTests.Controllers.Tasks
{
    using System.Threading.Tasks;
    using Task.API.Controllers;
    using Task.CQRS.Messages.Commands;
    using Task.CrossCutting.Exceptions;
    using Xunit;

    public class CreateTaskErrorReadonlyFieldShould
    {
        public CreateTaskErrorReadonlyFieldShould()
        {
            _controller = new TasksController(null, null, null);
        }

        private TasksController _controller { get; set; }

        private readonly CreateTaskCmd _cmd = new CreateTaskCmd
        {
            Id = 1
        };

        private readonly string _errors = "Create Task mustn't have 'id'";

        [Fact]
        public async Task Execute()
        {
            var result = await Assert.ThrowsAsync<CustomException>(async () => await _controller.Post(this._cmd, null));
            Assert.Equal(this._errors, result.Message);
        }
    }
}
namespace Task.UnitTests.Controllers.Tasks
{
    using System.Threading.Tasks;
    using Task.API.Controllers;
    using Task.CQRS.Messages.Commands;
    using Task.CrossCutting.Exceptions;
    using Xunit;

    public class UpdateTaskErrorReadonlyFieldShould
    {
        public UpdateTaskErrorReadonlyFieldShould()
        {
            _controller = new TasksController(null, null, null);
        }

        private TasksController _controller { get; set; }

        private readonly UpdateTaskCmd _cmd = new UpdateTaskCmd
        {
            Id = 1
        };

        private readonly string _errors = "Update Task mustn't have 'id'";

        private readonly int _id = 1;

        [Fact]
        public async Task Execute()
        {
            var result = await Assert.ThrowsAsync<CustomException>(async () => await _controller.Put(this._id, this._cmd, null));
            Assert.Equal(this._errors, result.Message);
        }
    }
}
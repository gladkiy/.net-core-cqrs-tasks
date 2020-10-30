namespace Task.UnitTests.Controllers.Tasks
{
    using System.Threading.Tasks;
    using Task.API.Controllers;
    using Task.CQRS.Messages.Commands;
    using Task.CrossCutting.Exceptions;
    using Xunit;

    public class DeleteTaskErrorShould
    {
        public DeleteTaskErrorShould()
        {
            _controller = new TasksController(null, null, null);
        }

        private TasksController _controller { get; set; }

        private readonly DeleteTaskCmd _cmd = new DeleteTaskCmd
        {
            Id = 3
        };

        private readonly string _errors = "id mismatch";

        private readonly int _id = 1;

        [Fact]
        public async Task Execute()
        {
            var result = await Assert.ThrowsAsync<CustomException>(async () => await _controller.Delete(this._id, this._cmd, null));
            Assert.Equal(this._errors, result.Message);
        }
    }
}
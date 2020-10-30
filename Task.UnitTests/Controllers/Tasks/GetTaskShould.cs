namespace Task.UnitTests.Controllers.Tasks
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Threading.Tasks;
    using Task.API.Controllers;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Queries;
    using Task.DTOModels;
    using Task.DTOModels.ViewModels;
    using Xunit;

    public class GetTaskShould
    {
        public GetTaskShould()
        {
            _dispatcher = new Mock<IDispatcher>();

            _dispatcher.Setup(r => r.QueryAsync(_queryHandler, this._query)).ReturnsAsync(this._response);

            _controller = new TasksController(null, _dispatcher.Object, null);
        }

        private TasksController _controller { get; set; }

        private Mock<IDispatcher> _dispatcher { get; set; }

        private IQueryHandlerAsync<GetTaskQuery, GetTaskQuery.Response> _queryHandler { get; set; }

        private readonly GetTaskQuery.Response _response = new GetTaskQuery.Response(new TaskVm
        {
            Id = 3,
            Description = "descr",
            Completed = false
        });

        private readonly GetTaskQuery _query = new GetTaskQuery
        {
            Id = 3
        };

        [Fact]
        public async Task Execute()
        {
            var result = await _controller.Get(this._query, _queryHandler) as JsonResult;
            var expectedResult = new Json(this._response.Task);

            Assert.IsType<JsonResult>(result);
            Assert.IsType<Json>(result.Value);
            Assert.NotNull(((Json)result.Value).Data);
            Assert.Equal(((TaskVm)((Json)result.Value).Data).Id, ((TaskVm)expectedResult.Data).Id);
            Assert.Equal(((TaskVm)((Json)result.Value).Data).Description, ((TaskVm)expectedResult.Data).Description);
            Assert.Equal(((TaskVm)((Json)result.Value).Data).Completed, ((TaskVm)expectedResult.Data).Completed);
        }
    }
}
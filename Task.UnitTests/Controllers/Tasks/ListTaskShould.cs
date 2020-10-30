namespace Task.UnitTests.Controllers.Tasks
{
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using Task.API.Controllers;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Queries;
    using Task.DTOModels;
    using Task.DTOModels.ViewModels;
    using Xunit;

    public class ListTaskShould
    {
        public ListTaskShould()
        {
            _dispatcher = new Mock<IDispatcher>();

            _dispatcher.Setup(r => r.Query(_queryHandler, this._query)).Returns(this._response);

            _controller = new TasksController(null, _dispatcher.Object, null);
        }

        private TasksController _controller { get; set; }

        private Mock<IDispatcher> _dispatcher { get; set; }

        private IQueryHandler<ListTaskQuery, ListTaskQuery.Response> _queryHandler { get; set; }

        private readonly ListTaskQuery.Response _response = new ListTaskQuery.Response(new List<TaskVm>
                                                                                       {
                                                                                           new TaskVm
                                                                                           {
                                                                                               Id = 3,
                                                                                               Description = "descr",
                                                                                               Completed = false
                                                                                           },
                                                                                           new TaskVm
                                                                                           {
                                                                                               Id = 4,
                                                                                               Description = "descr",
                                                                                               Completed = false
                                                                                           }
                                                                                       }, new Meta(2, 50, 1));

        private readonly ListTaskQuery _query = new ListTaskQuery
        {
            Sort = "id asc"
        };

        [Fact]
        public void Execute()
        {
            var result = _controller.List(this._query, _queryHandler) as JsonResult;
            var expectedResult = new Json(this._response.Tasks, this._response.Meta);

            Assert.IsType<JsonResult>(result);
            Assert.IsType<Json>(result.Value);
            Assert.NotNull(((Json)result.Value).Data);
            Assert.Equal(((Meta)((Json)result.Value).Meta).Total, ((Meta)expectedResult.Meta).Total);
            Assert.Equal(((Meta)((Json)result.Value).Meta).PageSize, ((Meta)expectedResult.Meta).PageSize);
            Assert.Equal(((Meta)((Json)result.Value).Meta).PageNumber, ((Meta)expectedResult.Meta).PageNumber);
            Assert.Equal(((List<TaskVm>)((Json)result.Value).Data).Count, ((List<TaskVm>)expectedResult.Data).Count);
            Assert.Equal(((List<TaskVm>)((Json)result.Value).Data).First().Id, ((List<TaskVm>)expectedResult.Data).First().Id);
            Assert.Equal(((List<TaskVm>)((Json)result.Value).Data).First().Description, ((List<TaskVm>)expectedResult.Data).First().Description);
            Assert.Equal(((List<TaskVm>)((Json)result.Value).Data).First().Completed, ((List<TaskVm>)expectedResult.Data).First().Completed);
        }
    }
}
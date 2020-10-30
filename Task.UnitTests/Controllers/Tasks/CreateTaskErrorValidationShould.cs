namespace Task.UnitTests.Controllers.Tasks
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Task.API.Controllers;
    using Task.CQRS.Messages.Commands;
    using Task.CrossCutting.Exceptions;
    using Xunit;

    public class CreateTaskErrorValidationShould
    {
        public CreateTaskErrorValidationShould()
        {
            _controller = new TasksController(null, null, null);

            _controller.ControllerContext = new ControllerContext();

            _controller.ModelState.AddModelError("Description", this._errors);
        }

        private TasksController _controller { get; set; }

        private readonly CreateTaskCmd _cmd = new CreateTaskCmd
        {
            Description = "dfg,dfgdfg,dqwe,egh,rtyjh,tyj,tyjtyjtyj,tyjtyj,tyjtwqw,wefwgrg,egrth,rergw,geht,rjt,fwe,ge,rhrt,hr,thrt,h,rth,rthr,th,rthrtherg,ergerg,ergergergerg,erge,wew,wefwef,ergergrt,rthrth,rthrthrh,ert,ertwer,wer,werwerw,erw,re,r,hg,j,uyr,g,dw,fe,gr,hty,gwefw,edf,w,fegr,erg,e,rge,rg"
        };

        private readonly string _errors = "The field Description must be a string with a maximum length of '255'.";

        [Fact]
        public async Task Execute()
        {
            var result = await Assert.ThrowsAsync<CustomException>(async () => await _controller.Post(this._cmd, null));
            Assert.Equal(this._errors, result.Message);
        }
    }
}
namespace Task.API.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;
    using Task.DBModels.Entities;

    public class ControllerBase : Controller
    {
        private readonly UserManager<User> _userManager;

        public ControllerBase(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<User> GetCurrentUserAsync()
        {
            return await this._userManager.GetUserAsync(User);
        }

        public string GetErrors()
        {
            return string.Join(", ", ModelState
                .Values
                .SelectMany(m => m.Errors)
                .Select(e => e.ErrorMessage)
                .ToList());
        }
    }
}
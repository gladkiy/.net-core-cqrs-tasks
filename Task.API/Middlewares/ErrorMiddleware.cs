namespace Task.API.Middlewares
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Task.CQRS.Impl;
    using Task.CQRS.Messages.Commands;
    using Task.CrossCutting.Enums;
    using Task.CrossCutting.Exceptions;
    using Task.DBModels.Entities;
    using Task.DTOModels;
    using Task = System.Threading.Tasks.Task;

    public class ErrorMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext httpContext,
            ILogger<ErrorMiddleware> logger,
            IDispatcher dispatcher,
            UserManager<User> userManager,
            ICommandHandlerAsync<AddErrorCmd> addErrorCmdHandler)
        {
            try
            {
                await this._next(httpContext);
            }
            catch (Exception exception)
            {
                var jsonErrors = new List<JsonError.ErrorItem>();

                var errorId = await LogError(httpContext, logger, dispatcher, userManager, addErrorCmdHandler, exception);

                if (exception is CustomException)
                {
                    var customException = (CustomException)exception;
                    jsonErrors.Add(new JsonError.ErrorItem(errorId, customException.Message));

                    switch ((ExceptionCode)customException.Code)
                    {
                        case ExceptionCode.Validation:
                            httpContext.Response.StatusCode = 400; //Bad Request
                            break;
                        case ExceptionCode.NotFound:
                            httpContext.Response.StatusCode = 404; //Not Found
                            break;
                        default:
                            httpContext.Response.StatusCode = 500; //Internal Server Error
                            break;
                    }
                }
                else
                {
                    jsonErrors.Add(new JsonError.ErrorItem(errorId, exception.Message));
                    httpContext.Response.StatusCode = 500; //Internal Server Error
                }

                var result = new JsonError(jsonErrors);
                var json = JsonConvert.SerializeObject(result);
                httpContext.Response.ContentType = "application/json";

                await httpContext.Response.WriteAsync(json).ConfigureAwait(false);
            }
        }

        private async Task<int?> GetUserId(HttpContext httpContext, UserManager<User> userManager)
        {
            var user = await userManager.GetUserAsync(httpContext.User);
            return user?.Id;
        }

        private async Task<int> LogError(HttpContext httpContext,
            ILogger<ErrorMiddleware> logger,
            IDispatcher dispatcher,
            UserManager<User> userManager,
            ICommandHandlerAsync<AddErrorCmd> addErrorCmdHandler,
            Exception exception)
        {
            var userId = await GetUserId(httpContext, userManager);

            var addErrorCmd = new AddErrorCmd();
            addErrorCmd.Exception = exception;
            addErrorCmd.Url = httpContext.Request.Path.Value + httpContext.Request.QueryString.Value;
            addErrorCmd.UserId = userId;
            await dispatcher.PushAsync(addErrorCmdHandler, addErrorCmd);
            var error = (ErrorLog)addErrorCmd.Result;
            logger.LogError($"id = {error.Id}"
                + $" | logged_date = {error.LoggedDate}"
                + $" | user_id = {error.UserId}"
                + $" | summary = {error.ErrorSummary}"
                + $" | location = {error.ErrorLocation}"
                + $" | url = {error.Url}");

            return error.Id;
        }
    }
}
namespace Task.CQRS.Messages.Commands
{
    using System;
    using Task.CQRS.Impl;
    using Task.DBModels.Entities;

    public class AddErrorCmd : CommandBase
    {
        public Exception Exception { get; set; }

        public string Url { get; set; }

        public int? UserId { get; set; }

        public ErrorLog ToErrorLog()
        {
            return new ErrorLog
            {
                LoggedDate = DateTime.UtcNow,
                Url = Url,
                ErrorSummary = Exception.Message,
                ErrorDetail = Exception.StackTrace,
                ErrorLocation = Exception.StackTrace.Split('\n')[0],
                UserId = UserId
            };
        }
    }
}
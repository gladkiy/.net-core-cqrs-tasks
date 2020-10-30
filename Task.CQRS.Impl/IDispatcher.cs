namespace Task.CQRS.Impl
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDispatcher
    {
        void Push<TCommand>(ICommandHandler<TCommand> commandHandler, TCommand message);

        Task PushAsync<TCommand>(ICommandHandlerAsync<TCommand> commandHandlerAsync, TCommand message);

        TResponse Query<TRequest, TResponse>(IQueryHandler<TRequest, TResponse> queryHandler, TRequest message);

        Task<TResponse> QueryAsync<TRequest, TResponse>(IQueryHandlerAsync<TRequest, TResponse> queryHandlerAsync, TRequest message);

        void PushBatch<TCommand>(Dictionary<TCommand, ICommandHandler<TCommand>> messagesAndHandlers);

        Task PushBatchAsync<TCommand>(Dictionary<TCommand, ICommandHandlerAsync<TCommand>> messagesAndHandlers);
    }
}
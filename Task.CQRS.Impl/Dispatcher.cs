namespace Task.CQRS.Impl
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Dispatcher : IDispatcher
    {
        public void Push<TCommand>(ICommandHandler<TCommand> commandHandler, TCommand message)
        {
            commandHandler.Handle(message);
        }

        public async Task PushAsync<TCommand>(ICommandHandlerAsync<TCommand> commandHandlerAsync, TCommand message)
        {
            await commandHandlerAsync.HandleAsync(message);
        }

        public TResponse Query<TRequest, TResponse>(IQueryHandler<TRequest, TResponse> queryHandler, TRequest message)
        {
            return queryHandler.Handle(message);
        }

        public async Task<TResponse> QueryAsync<TRequest, TResponse>(IQueryHandlerAsync<TRequest, TResponse> queryHandlerAsync, TRequest message)
        {
            return await queryHandlerAsync.HandleAsync(message);
        }

        public void PushBatch<TCommand>(Dictionary<TCommand, ICommandHandler<TCommand>> messagesAndHandlers)
        {
            foreach (var messageAndHandler in messagesAndHandlers)
            {
                messageAndHandler.Value.Handle(messageAndHandler.Key);
            }
        }

        public async Task PushBatchAsync<TCommand>(Dictionary<TCommand, ICommandHandlerAsync<TCommand>> messagesAndHandlers)
        {
            foreach (var messageAndHandler in messagesAndHandlers)
            {
                await messageAndHandler.Value.HandleAsync(messageAndHandler.Key);
            }
        }
    }
}
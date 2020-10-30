namespace Task.CQRS.Impl
{
    using System.Threading.Tasks;

    public interface ICommandHandlerAsync<T>
    {
        Task HandleAsync(T command);
    }
}
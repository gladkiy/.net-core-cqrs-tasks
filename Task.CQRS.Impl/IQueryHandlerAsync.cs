namespace Task.CQRS.Impl
{
    using System.Threading.Tasks;

    public interface IQueryHandlerAsync<TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(TRequest message);
    }
}
namespace Task.CQRS.Impl
{
    public interface IQueryHandler<TRequest, TResponse>
    {
        TResponse Handle(TRequest message);
    }
}
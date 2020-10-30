namespace Task.CQRS.Impl
{
    public interface ICommandHandler<T>
    {
        void Handle(T command);
    }
}
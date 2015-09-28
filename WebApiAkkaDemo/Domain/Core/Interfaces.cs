namespace WebApiAkkaDemo.Domain.Core
{
    /// <summary>
    /// marks a command message
    /// </summary>
    public interface ICommand {}

    /// <summary>
    /// marks an event message
    /// </summary>
    public interface IEvent {}

    /// <summary>
    /// a command message addressed to an aggregate by its Id
    /// </summary>
    public interface IAddressed<TId> : ICommand
    {
        TId Id { get; set; }
    }
}

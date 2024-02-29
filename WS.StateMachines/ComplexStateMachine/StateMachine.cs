namespace WS.StateMachines.ComplexStateMachine;

public class StateMachine<T>
{
    private readonly ImmutableList<IHandler<T>> handlers;

    public StateMachine(params IHandler<T>[] handlers)
    {
        this.handlers = handlers.ToImmutableList();
    }

    public StateMachine(IEnumerable<IHandler<T>> handlers) : this(handlers.ToArray())
    {
    }

    public T ProcessMessage(T state, object message)
    {
        var possibleHandlers = GetPossibleHandlers(state, message);
        if (possibleHandlers.Count != 1)
        {
            throw new ArgumentException($"No distinct handler found for supplied state and message", nameof(message));
        }
        return possibleHandlers[0].Process(state, message);
    }

    public IEnumerable<(Type Type, string Description)> GetPossibleMessageTypes(T state)
    {
        return handlers
            .Where(h => h.CanProcess(state))
            .Select(h => (h.MessageType, h.Description))
            .Distinct();
    }

    public bool CanProcess(T state, object message)
    {
        return GetPossibleHandlers(state, message).Count == 1;
    }

    private List<IHandler<T>> GetPossibleHandlers(T state, object message)
    {
        return handlers
            .Where(h => h.CanProcess(state) && h.CanProcess(state, message))
            .ToList();
    }
}

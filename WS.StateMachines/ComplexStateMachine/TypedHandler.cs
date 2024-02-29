namespace WS.StateMachines.ComplexStateMachine;

public abstract class TypedHandler<TState, TMessage> : IHandler<TState>
{
    public abstract string Description { get; }
    protected abstract TState Process(TState state, TMessage message);
    protected abstract bool CanProcess(TState state, TMessage message);

    public Type MessageType => typeof(TMessage);

    public abstract bool CanProcess(TState state);

    public bool CanProcess(TState state, object message)
    {
        if (CanProcess(state) && message is TMessage typedMessage)
        {
            return CanProcess(state, typedMessage);
        }
        return false;
    }

    public TState Process(TState state, object message)
    {
        if (CanProcess(state, message))
        {
            return Process(state, (TMessage)message);
        }
        throw new ArgumentException($"Cannot handle supplied state and message", nameof(message));
    }
}

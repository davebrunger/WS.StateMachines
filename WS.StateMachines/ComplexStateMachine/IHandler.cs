namespace WS.StateMachines.ComplexStateMachine;

public interface IHandler<T>
{
    public string Description { get; }
    public T Process(T state, object message);
    public Type MessageType { get; }
    public bool CanProcess(T state);
    public bool CanProcess(T state, object message);
}

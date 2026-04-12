using R3;
using System;

public abstract class BaseState : IGameState, IDisposable
{
    protected CompositeDisposable Disposables { get; } = new();

    public virtual void Enter(){}

    public virtual void Exit(){}

    public virtual void Dispose() => Disposables.Dispose();
}

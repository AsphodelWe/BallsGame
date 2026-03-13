using Unity.VisualScripting;
using UnityEngine;
using R3;

public abstract class BaseState : IGameState
{
    protected CompositeDisposable Disposables { get; } = new();

    public virtual void Enter(){}

    public virtual void Exit(){}

    public virtual void Dispose() => Disposables.Dispose();
}

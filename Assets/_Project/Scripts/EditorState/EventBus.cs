using R3;
using UnityEngine;

public class EventBus : IEventBus
{
    public Subject<Unit> OnBattleClicked { get; } = new();
    public ReactiveProperty<Collider2D> OnMapBoundsReady { get; } = new();
}

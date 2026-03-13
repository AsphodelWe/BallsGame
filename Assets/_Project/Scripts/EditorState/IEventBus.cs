using UnityEngine;
using R3;

public interface IEventBus
{
    Subject<Unit> OnBattleClicked { get; }
    ReactiveProperty<Collider2D> OnMapBoundsReady { get; }
}

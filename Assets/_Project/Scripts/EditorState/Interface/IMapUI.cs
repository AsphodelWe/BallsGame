using UnityEngine;
using R3;
public interface IMapUI
{
    Subject<Unit> OnNextStage { get; }
    Subject<Unit> OnNextClicked { get; }
    Subject<Unit> OnPreviousClicked { get; }
    void Show();
    void Hide();
}

using UnityEngine;
using R3;
using System;
public interface IUnitPlacementUI
{
    public Subject<CountryConfig> OnSelectCountry { get; }
    public Subject<float> OnScaleChanged { get; }
    public Subject<Unit> OnClearGhosts { get; }
    public Subject<bool> OnRotateToggleChanged { get; }
    CountryConfig CurrentSelectedCountry { get; }
    void SetActiveBattleButton(bool flag);
    void Show();
    void Hide();
}

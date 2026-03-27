using UnityEngine;
using R3;
using System;
public interface IUnitPlacementUI
{
    public event Action<CountryConfig> OnSelectCountry;
    public event Action<float> OnScaleChanged;
    public event Action OnClearGhosts;
    public event Action<bool> OnRotateToggleChanged;
    void SetActiveBattleButton(bool flag);
    void Show();
    void Hide();
}

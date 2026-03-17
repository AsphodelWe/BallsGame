using UnityEngine;
using UnityEngine.UIElements;

public interface IUiStrategy
{
    void Initialize(VisualTreeAsset healthBarTemplate, BattleUISettings UIsettings, UIDocument uiDoc);
    void UpdateHealthBar(int current, int max, VisualElement fill, Label text);
    void UpdateBarsAlignment();
}

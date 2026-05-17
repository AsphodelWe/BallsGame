using UnityEngine;
using UnityEngine.UIElements;

public interface IWeaponPanel
{
    AttackWeaponType Type { get; }
    void Initialize(VisualElement root);
    void LoadFrom(IShootConfig module);
    void SaveTo(IShootConfig module);
    void Show(bool show);
}

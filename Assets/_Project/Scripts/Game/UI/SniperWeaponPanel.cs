using UnityEngine;
using UnityEngine.UIElements;

public class SniperWeaponPanel : IWeaponPanel
{
    public AttackWeaponType Type => AttackWeaponType.Sniper;
    
    private VisualElement _root;
    private Slider _fireRateSlider;
    private Slider _recoilSlider;
    private Toggle _markTargetToggle;
    
    public void Initialize(VisualElement container)
    {
        _root = container.Q<VisualElement>("SniperWeapon");

        _fireRateSlider = _root.Q<Slider>("SniperFireRateSlider");
        _recoilSlider = _root.Q<Slider>("SniperRecoilSlider");
        _markTargetToggle = _root.Q<Toggle>("SniperEnableMark");
    }
    
    public void LoadFrom(IShootConfig module)
    {
        if (module is SniperShootConfig sniper)
        {
            _fireRateSlider.value = sniper.FireRate;
            _recoilSlider.value = sniper.RecoilForce;
        }
    }
    
    public void SaveTo(IShootConfig module)
    {
        if (module is SniperShootConfig sniper)
        {
            sniper.FireRate = _fireRateSlider.value;
            sniper.RecoilForce = _recoilSlider.value;
        }
    }
    
    public void Show(bool show)
    {
        _root.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }
}

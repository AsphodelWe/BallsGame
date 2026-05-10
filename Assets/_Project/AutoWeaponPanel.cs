using UnityEngine;
using UnityEngine.UIElements;

public class AutoWeaponPanel : IWeaponPanel
{
    public AttackWeaponType Type => AttackWeaponType.Auto;

    private VisualElement _root;
    private Slider _fireRateSlider;
    private SliderInt _reloadTimeSlider;
    private SliderInt _maxAmmoSlider;
    private Slider _recoilSlider;
    private Slider _hitForce;

    public void Initialize(VisualElement root)
    {
        _root = root.Q<VisualElement>("AutoWeapon");
        _fireRateSlider = _root.Q<Slider>("AutoFireRateSlider");
        _reloadTimeSlider = _root.Q<SliderInt>("AutoReloadTimeSlider");
        _maxAmmoSlider = _root.Q<SliderInt>("AutoMaxAmmoSlider");
        _recoilSlider = _root.Q<Slider>("AutoRecoilSlider");
        _hitForce = root.Q<Slider>("AutoHitForceSlider");
    }

    public void LoadFrom(IShootConfig module)
    {
        if (module is AutoWeaponConfig auto)
        {
            
            _fireRateSlider.value = auto.FireRate;
            _reloadTimeSlider.value = (int)auto.ReloadTime;
            _maxAmmoSlider.value = auto.MaxAmmo;
            _recoilSlider.value = auto.RecoilForce;
            _hitForce.value = auto.HitForce;
        }
    }

    public void SaveTo(IShootConfig module)
    {
        if (module is AutoWeaponConfig auto)
        {
            auto.FireRate = _fireRateSlider.value;
            auto.ReloadTime = _reloadTimeSlider.value;
            auto.MaxAmmo = _maxAmmoSlider.value;
            auto.RecoilForce = _recoilSlider.value;
            auto.HitForce = _hitForce.value;
        }
    }

    public void Show(bool show)
    {
        _root.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }
}

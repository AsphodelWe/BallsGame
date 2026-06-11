using UnityEngine;
using UnityEngine.UIElements;

public class SingleWeaponPanel : IWeaponPanel
{
    public AttackWeaponType Type => AttackWeaponType.Single;

    private VisualElement _root;
    private Slider _fireRateSlider;
    private Slider _recoilSlider;
    private Slider _hitForceSlider;

    public void Initialize(VisualElement root)
    {
        _root = root.Q<VisualElement>("SingleWeapon");

        _fireRateSlider = _root.Q<Slider>("SingleFireRateSlider");
        _recoilSlider = _root.Q<Slider>("SingleRecoilSlider");
        _hitForceSlider = _root.Q<Slider>("SingleHitForceSlider");
    }

    public void LoadFrom(IShootConfig module)
    {
        if (module is SingleShootConfig single)
        {
            _fireRateSlider.value = single.FireRate;
            _recoilSlider.value = single.RecoilForce;
            _hitForceSlider.value = single.HitForce;
        }
    }

    public void SaveTo(IShootConfig module)
    {
        if (module is SingleShootConfig single)
        {
            single.FireRate = _fireRateSlider.value;
            single.RecoilForce = _recoilSlider.value;
            single.HitForce = _hitForceSlider.value;
        }
    }

    public void Show(bool show)
    {
        _root.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }
}

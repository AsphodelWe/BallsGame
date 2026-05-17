using UnityEngine;
using UnityEngine.UIElements;

public class ShotgunWeaponPanel : IWeaponPanel
{
    public AttackWeaponType Type => AttackWeaponType.Shotgun;

    private VisualElement _root;
    private Slider _fireRateSlider;
    private SliderInt _bulletCountSlider;
    private SliderInt _spreadAngleSlider;
    private Slider _recoilSlider;
    private Slider _hitForceSlider;

    public void Initialize(VisualElement root)
    {
        _root = root.Q<VisualElement>("ShotgunWeapon");

        _fireRateSlider = _root.Q<Slider>("ShotgunReloadTimeSlider");
        _bulletCountSlider = root.Q<SliderInt>("ShotgunSizeSlider");
        _spreadAngleSlider = root.Q<SliderInt>("ShotgunSpreadAngleSlider");
        _recoilSlider = _root.Q<Slider>("ShotgunRecoilSlider");
        _hitForceSlider = _root.Q<Slider>("ShotgunHitForceSlider");
    }
    public void LoadFrom(IShootConfig module)
    {
        if (module is ShotgunConfig shotgun)
        {
            _fireRateSlider.value = shotgun.FireRate;
            _bulletCountSlider.value = shotgun.BulletCount;
            _spreadAngleSlider.value = shotgun.SpreadAngle;
            _recoilSlider.value = shotgun.RecoilForce;
            _hitForceSlider.value = shotgun.HitForce;
        }
    }

    public void SaveTo(IShootConfig module)
    {
        if (module is ShotgunConfig shotgun)
        {
            shotgun.FireRate = _fireRateSlider.value;
            shotgun.BulletCount = _bulletCountSlider.value;
            shotgun.SpreadAngle = _spreadAngleSlider.value;
            shotgun.RecoilForce = _recoilSlider.value;
            shotgun.HitForce = _hitForceSlider.value;
        }
    }

    public void Show(bool show)
    {
        _root.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }
}

using Unity.Burst;
using UnityEngine;
using UnityEngine.UIElements;

public class BurstWeaponPanel : IWeaponPanel
{
    public AttackWeaponType Type => AttackWeaponType.Burst;
    private VisualElement _root;
    private Slider _bulletDelaySlider;
    private Slider _burstReloadTime;
    private SliderInt _burstSizeSlider;
    private Slider _burstRecoilSlider;
    private Slider _burstHitForceSlider;

    public void Initialize(VisualElement root)
    {
        _root = root.Q<VisualElement>("BurstWeapon");

        _bulletDelaySlider = _root.Q<Slider>("BurstDelaySlider");
        _burstReloadTime = _root.Q<Slider>("BurstReloadTimeSlider");
        _burstSizeSlider = _root.Q<SliderInt>("BurstSizeSlider");
        _burstRecoilSlider = _root.Q<Slider>("BurstRecoilSlider");
        _burstHitForceSlider = _root.Q<Slider>("BurstHitForceSlider");
    }
    public void LoadFrom(IShootConfig module)
    {
        if (module is BurstWeaponConfig burst)
        {
            _bulletDelaySlider.value = burst.BulletDelay;
            _burstReloadTime.value = burst.BurstDelay;
            _burstSizeSlider.value = burst.BurstSize;
            _burstRecoilSlider.value = burst.RecoilForce;
            _burstHitForceSlider.value = burst.HitForce;
        }
    }

    public void SaveTo(IShootConfig module)
    {
        if (module is BurstWeaponConfig burst)
        {
            burst.BulletDelay = _bulletDelaySlider.value;
            burst.BurstDelay = _burstReloadTime.value;
            burst.BurstSize = _burstSizeSlider.value;
            burst.RecoilForce = _burstRecoilSlider.value;
            burst.HitForce = _burstHitForceSlider.value;
        }
    }

    public void Show(bool show)
    {
        _root.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    }
}

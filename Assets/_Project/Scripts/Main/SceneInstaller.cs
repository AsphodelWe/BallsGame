using UnityEngine;
using Reflex.Core;
using Reflex.Enums;
using System;


public class SceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private UnitPlacementView _unitPlaceView;
    [SerializeField] private UnitPlacementUI _unitUI;
    [SerializeField] private ChooseMapUI _chooseMapUI;
    [SerializeField] private MapSelectionView _mapView;
    [SerializeField] private MapRegistry _mapRegistry;
    [SerializeField] private MobileUnitPlacementView _mobileUnitPlaceView;
    [SerializeField] private WebGLUnitPlacementView _webGLUnitPlaceView;
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.RegisterType(typeof(BuildController), Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(UnitPlacementState), Lifetime.Transient, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(MapSelectionState), Lifetime.Transient, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(EditorState), Lifetime.Transient, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(BattleState), Lifetime.Transient, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(MapScaler), Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(GhostFactory), Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(EventBus), new Type[] { typeof(IEventBus) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);

        builder.RegisterValue(_mapRegistry);

        builder.RegisterValue(_chooseMapUI, new Type[] { typeof(IMapUI) });
        builder.RegisterValue(_mapView, new Type[] { typeof(IMapView) });

        builder.RegisterValue(_unitUI, new Type[] { typeof(IUnitPlacementUI) });


#if UNITY_ANDROID || UNITY_IOS
       builder.RegisterValue(_mobileUnitPlaceView, new Type[] { typeof(IUnitPlacementView) });
#elif UNITY_WEBGL
        builder.RegisterValue(_webGLUnitPlaceView, new Type[] { typeof(IUnitPlacementView) });
#else
    builder.RegisterValue(_unitPlaceView, new Type[] { typeof(IUnitPlacementView) });
#endif

    }
}

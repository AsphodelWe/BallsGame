using System;
using Reflex.Core;
using Reflex.Enums;
using UnityEngine;

public class BattleSceneInstaller : MonoBehaviour, IInstaller
{
        [SerializeField] private BattleUI _battleUI;
        [SerializeField] private UIStrategyController _uiController;

        [SerializeField] private MobileInputHandler _mobileInputHandler;
        public void InstallBindings(ContainerBuilder builder)
        {

                builder.RegisterType(typeof(MasterFactory), new Type[] { typeof(IMasterFactory) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
                builder.RegisterType(typeof(WeaponFactory), new Type[] { typeof(AttackerFactory) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
                builder.RegisterType(typeof(WeaponAttackHandlerFactory), new Type[] { typeof(IWeaponAttackHandlerFactory) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
                builder.RegisterType(typeof(TargetStrategyFactory), new Type[] { typeof(ITargetStrategyFactory) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
                builder.RegisterType(typeof(UiStrategyFactory), Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
                builder.RegisterType(typeof(BallFactory), Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);


#if UNITY_ANDROID || UNITY_IOS
                builder.RegisterValue(_mobileInputHandler, new Type[] { typeof(IInputHandler) });
#else
        builder.RegisterType(typeof(KeyboardInputHandler), new Type[] { typeof(IInputHandler) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
#endif


                builder.RegisterType(typeof(TimeManager), new Type[] { typeof(ITimeManager) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);

                builder.RegisterType(typeof(BallRegistry), Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);

                builder.RegisterValue(_battleUI);
                builder.RegisterValue(_uiController);
        }
}

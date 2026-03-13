using System;
using Reflex.Core;
using Reflex.Enums;
using UnityEngine;

public class BattleSceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private CameraBattleController _camera;
    public void InstallBindings(ContainerBuilder builder)
    {
        builder.RegisterType(typeof(MasterFactory), new Type[] { typeof(IMasterFactory) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(WeaponFactory), new Type[] { typeof(AttackerFactory) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(WeaponAttackHandlerFactory), new Type[] { typeof(IWeaponAttackHandlerFactory) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(TargetStrategyFactory), new Type[] { typeof(ITargetStrategyFactory) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);

        builder.RegisterType(typeof(BallFactory), Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
        builder.RegisterType(typeof(BallRegistry), Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
        
        builder.RegisterValue(_camera);

    }
}

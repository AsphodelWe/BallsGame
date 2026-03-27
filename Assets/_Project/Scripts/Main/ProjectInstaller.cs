using UnityEngine;
using Reflex.Core;
using Reflex.Enums;
using System;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private BattleData _battleData;
    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterValue(_battleData);
        containerBuilder.RegisterType(typeof(MapScaleProvider), new Type[] { typeof(IMapScaleProvider) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);
    }
}

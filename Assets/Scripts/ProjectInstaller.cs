using UnityEngine;
using Reflex.Core;
using Reflex.Enums;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private BattleData _battleData;
    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterValue(_battleData);
    }
}

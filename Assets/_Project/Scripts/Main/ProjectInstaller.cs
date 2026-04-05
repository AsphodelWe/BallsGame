using UnityEngine;
using Reflex.Core;
using Reflex.Enums;
using System;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private BattleData _battleData;
    [SerializeField] private CameraController _cameraPrefab;
    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterValue(_battleData);
        containerBuilder.RegisterType(typeof(MapScaleProvider), new Type[] { typeof(IMapScaleProvider) }, Lifetime.Singleton, Reflex.Enums.Resolution.Lazy);

        if (_cameraPrefab != null)
        {
            var camera = Instantiate(_cameraPrefab);
            DontDestroyOnLoad(camera.gameObject);
            containerBuilder.RegisterValue(camera);

            // Убеждаемся, что у камеры есть CameraController скрипт
            var cameraController = camera.GetComponent<CameraController>();
            if (cameraController == null)
            {
                cameraController = camera.gameObject.AddComponent<CameraController>();
            }
            containerBuilder.RegisterValue(cameraController);
        }
        else
        {
            Debug.LogError("Camera prefab not assigned in ProjectInstaller!");
        }
    }
}

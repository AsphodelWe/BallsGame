using UnityEngine;
using Reflex.Attributes;
using Reflex.Core;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Reflex.Extensions;
using System.Threading;
using System;
using Reflex.Injectors;
public class BattleState : BaseState
{
    [Inject] private BattleData _battleData;
    [Inject] private Container _battleContainer;
    private BallFactory _ballFactory;
    private CancellationTokenSource _cts;

    private const string _battleSceneName = "BattleScene";
    private readonly float ZOOM = 4f;

    public override void Enter()
    {
        _cts = new CancellationTokenSource();
        LoadBattleAsync(_cts).Forget();
    }
    private async UniTaskVoid LoadBattleAsync(CancellationTokenSource сts)
    {
        try
        {
            await SceneManager.LoadSceneAsync(_battleSceneName).ToUniTask();
            _battleContainer = SceneManager.GetActiveScene().GetSceneContainer();

            SetZoom();
            SpawnMap();
            CreateFabric();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Загрузка битвы отменена");
        }
    }

    private void SpawnMap()
    {
        GameObject map = UnityEngine.Object.Instantiate(_battleData.SelectedMap.BattlePrefab);
        float scale = _battleData.MapScale;
        map.transform.localScale = new Vector3(scale, scale, scale);
        GameObjectInjector.InjectRecursive(map, _battleContainer);
    }

    private void SetZoom()
    {
        var _camera = _battleContainer.Resolve<CameraBattleController>();
        float mapScale = _battleData.MapScale;
        _camera.SetCameraZoom(_battleData.MapScale * ZOOM);
    }

    private void CreateFabric()
    {
        _ballFactory = _battleContainer.Resolve<BallFactory>();

        foreach (var ghost in _battleData.GhostList)
        {
            if (ghost?.Country == null) continue;
            _ballFactory.CreateBall(ghost.Country, ghost.Position);
        }
    }

    public override void Exit()
    {
        _cts?.Cancel();
        _cts.Dispose();
        base.Exit();
    }
}

using UnityEngine;
using Reflex.Attributes;
using Reflex.Core;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Reflex.Extensions;
using System.Threading;
using System;
using Reflex.Injectors;
using DG.Tweening;
public class BattleState : BaseState
{
    [Inject] private BattleData _battleData;
    [Inject] private Container _battleContainer;
    [Inject] private CameraController _cameraController;
    private InputHandlerBattle _inputHandler;
    private BattleUI _battleUI;
    private UIStrategyController _uiController;
    private BallFactory _ballFactory;
    private CancellationTokenSource _cts;
    private GameObject map;
    private const string _battleSceneName = "BattleScene";

    public override void Enter()
    {
        _cts = new CancellationTokenSource();
        _cameraController.SetBattlePosition();
        _cameraController.BeginBob();
        LoadBattleAsync(_cts).Forget();
    }
    private async UniTaskVoid LoadBattleAsync(CancellationTokenSource сts)
    {
        try
        {
            await SceneManager.LoadSceneAsync(_battleSceneName).ToUniTask();
            _battleContainer = SceneManager.GetActiveScene().GetSceneContainer();

            _inputHandler = _battleContainer.Resolve<InputHandlerBattle>();
            _inputHandler.OnBattleRestart += Reset;
            _inputHandler.OnExitToMenu += Exit;

            _uiController = _battleContainer.Resolve<UIStrategyController>();

            SpawnMap();
            CreateFabric();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Загрузка битвы отменена");
        }

        _battleUI = _battleContainer.Resolve<BattleUI>();
        _battleUI.Initialize();
        _battleUI.SetPause();
    }

    private void SpawnMap()
    {
        map = UnityEngine.Object.Instantiate(_battleData.SelectedMap.BattlePrefab);
        float scale = _battleData.MapScale;
        map.transform.localScale = new Vector3(scale, scale, scale);

        var globalContainer = Container.RootContainer;
        GameObjectInjector.InjectRecursive(map, globalContainer);
    }

    private void CreateFabric()
    {
        _ballFactory = _battleContainer.Resolve<BallFactory>();
        CreateBalls();
    }

    private void RestartLevel()
    {
        ClearResourses();

        SpawnMap();
        CreateBalls();

        _uiController.ResetUI();

        _cameraController.SetBattlePosition();
        _cameraController.BeginBob();

        _battleUI.Initialize();
        _battleUI.SetPause();
        _battleUI.SetVisualStartBattleButton();

    }

    private void Reset() => RestartLevel();

    private void CreateBalls()
    {
        foreach (var ghost in _battleData.GhostList)
        {
            if (ghost?.Country == null) continue;
            _ballFactory.CreateBall(ghost.Country, ghost.Position);
        }
    }

    private async UniTask ExitToMenu()
    {
        ClearResourses();
        _battleData.RemoveAllGhost();

        if (_inputHandler != null)
        {
            _inputHandler.OnBattleRestart -= Reset;
            _inputHandler.OnExitToMenu -= Exit;
        }

        _cts?.Cancel();
        _cts?.Dispose();

        await UniTask.Yield();

        _cameraController.SetEditorPosition();
        _cameraController.SetEditorZoom();

        SceneManager.LoadScene("SampleScene");
    }

    private void ClearResourses()
    {
        DOTween.KillAll();

        if (map != null)
            UnityEngine.Object.Destroy(map);

        _ballFactory?.DestroyAll();
    }

    public override void Exit() => ExitToMenu().Forget();

}

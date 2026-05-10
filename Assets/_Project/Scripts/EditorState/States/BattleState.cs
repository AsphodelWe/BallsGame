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
    [Inject] private CameraController _cameraController;
    private IInputHandler _inputHandler;
    private BattleUI _battleUI;
    private UIStrategyController _uiController;
    private BallFactory _ballFactory;
    private CancellationTokenSource _cts;
    private GameObject map;
    private ITimeManager _timeManager;
    private const string _battleSceneName = "BattleScene";

    public override void Enter()
    {
        _cts = new CancellationTokenSource();
        _cameraController.SetBattlePosition();
        LoadBattleAsync(_cts).Forget();
    }
    private async UniTaskVoid LoadBattleAsync(CancellationTokenSource сts)
    {
        try
        {
            await SceneManager.LoadSceneAsync(_battleSceneName).ToUniTask();
            _battleContainer = SceneManager.GetActiveScene().GetSceneContainer();
            _timeManager = _battleContainer.Resolve<ITimeManager>();

            SetupInputHandler();
            SetupUI();
            SpawnMap();

            _ballFactory = _battleContainer.Resolve<BallFactory>();
            CreateBalls();
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Загрузка битвы отменена");
        }

        _timeManager.Pause();
    }

    private void SpawnMap()
    {
        map = UnityEngine.Object.Instantiate(_battleData.SelectedMap.BattlePrefab);
        float scale = _battleData.MapScale;
        map.transform.localScale = new Vector3(scale, scale, scale);

        var globalContainer = Container.RootContainer;
        GameObjectInjector.InjectRecursive(map, globalContainer);
    }

    private void RestartLevel()
    {
        ClearResources();

        SpawnMap();
        CreateBalls();

        _uiController.ResetUI();

        _cameraController.SetBattlePosition();

        _timeManager.Pause();
        _battleUI.SetStartButtonVisible(true);
        _battleUI.SetCursorVisible(true);

    }

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

        ClearResources();
        _battleData.RemoveAllGhost();

        await UniTask.Yield();

        _battleUI.SetCursorVisible(true);

        _cameraController.SetEditorPosition();
        _cameraController.SetEditorZoom();

        SceneManager.LoadScene("SampleScene");
    }

    private void ClearResources()
    {
        if (map != null)
            UnityEngine.Object.Destroy(map);

        _ballFactory?.DestroyAll();
    }

    private void SetupInputHandler()
    {
        _inputHandler = _battleContainer.Resolve<IInputHandler>();
        _inputHandler.OnBattleRestart += () => { Reset(); };
        _inputHandler.OnExitToMenu += () => { Exit(); };
        _inputHandler.Enable();
    }

    private void SetupUI()
    {
        _uiController = _battleContainer.Resolve<UIStrategyController>();
        _battleUI = _battleContainer.Resolve<BattleUI>();
        _battleUI.OnBattleStarted += () => _timeManager.Resume();
    }

    public override void Exit() => ExitToMenu().Forget();
    private void Reset() => RestartLevel();
    public override void Dispose()
    {
        if (_inputHandler != null)
        {
            _inputHandler.OnBattleRestart -= () => { Reset(); };
            _inputHandler.OnExitToMenu -= () => { Exit(); };
            _inputHandler.Disable();
            (_inputHandler as IDisposable)?.Dispose();
        }

        _cts?.Cancel();
        _cts?.Dispose();

        _uiController?.ResetUI();
        (_uiController as IDisposable)?.Dispose();

        base.Dispose();
    }
}

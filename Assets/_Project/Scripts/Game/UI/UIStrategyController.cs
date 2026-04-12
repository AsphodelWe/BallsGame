using System;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

public class UIStrategyController : MonoBehaviour
{
    [Inject] private UiStrategyFactory _factory;
    [Inject] private BallRegistry _ballRegistry;
    [SerializeField] private VisualTreeAsset _healthBarTemplate;
    [SerializeField] private BattleUISettings _uiSettings;

    private IUiStrategy _currentStrategy;
    private UIDocument _uiDoc;

    private void Start()
    {
        if (_uiSettings == null)
        {
            Debug.LogError("BattleUISettings not assigned!", this);
            return;
        }

        _uiDoc = GetComponent<UIDocument>();
        if (_uiDoc == null)
        {
            Debug.LogError("UIDocument component missing!", this);
            return;
        }

        ApplyStrategy();
    }

    private void ApplyStrategy()
    {
        _currentStrategy = _factory.CreateUIStratege();
        _currentStrategy.Initialize(_healthBarTemplate, _uiSettings, _uiDoc);

        switch (_currentStrategy)
        {
            case IndividualHealthUI individual:
                foreach (var ball in _ballRegistry.GetAllBalls())
                    individual.SetBallHealth(ball);
                break;
            case TeamHealthUI team:
                foreach (var side in team.TeamSides)
                    team.SetTeamHealth(side);
                break;
        }
    }

    public void ResetUI()
    {
        var barsContainer = _uiDoc.rootVisualElement.Q<VisualElement>("HealthBarsContainer");
        barsContainer?.Clear();

        _currentStrategy?.Clear();

        ApplyStrategy();
        _currentStrategy?.UpdateBarsAlignment();
    }

    private void OnDestroy()
    {
        (_currentStrategy as IDisposable)?.Dispose();
    }
}



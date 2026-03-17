using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

public class UIStrategyController : MonoBehaviour
{
    [Inject] private UiStrategyFactory factory;
    [Inject] private BallRegistry _ballRegistry;
    [SerializeField] private VisualTreeAsset _healthBarTemplate;
    [SerializeField] private BattleUISettings _uIsettings;
    private IUiStrategy _currentStrategy;
    private UIDocument _uiDoc;

    private void Start()
    {
        if (_uIsettings == null)
        {
            Debug.LogError("BattleUISettings not assigned!", this);
            return;
        }

        _uiDoc = GetComponent<UIDocument>();

        ApplyStrategy();
    }

    private void ApplyStrategy()
    {
        _currentStrategy = factory.CreateUIStratege();
        _currentStrategy.Initialize(_healthBarTemplate, _uIsettings, _uiDoc);

        if (_currentStrategy is IndividualHealthUI individual)
        {
            foreach (var ball in _ballRegistry.GetAllBalls())
                individual.SetBallHealth(ball);
        }
        else if (_currentStrategy is TeamHealthUI team)
        {
            foreach (var side in team.TeamSides)
                team.SetTeamHealth(side);
        }
    }
}



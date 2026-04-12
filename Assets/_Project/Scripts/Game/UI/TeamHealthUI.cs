using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class TeamData
{
    public VisualElement Bar;
    public List<BallPresent> Balls;
    public int MaxHealth;
    public int CurrentHealth;
}
public class TeamHealthUI : IUiStrategy
{
    private BallRegistry _ballRegistry;
    private VisualTreeAsset _healthBarTemplate;
    private BattleUISettings _UIsettings;
    private VisualElement _root;
    private VisualElement _barsContainer;
    private Dictionary<SideConfig, TeamData> _teams = new();
    public IEnumerable<SideConfig> TeamSides => _teams.Keys;
    private Dictionary<BallPresent, Action<int>> _damageHandlers = new();
    private Dictionary<BallPresent, Action> _destroyHandlers = new();

    public TeamHealthUI(BallRegistry ballRegistry)
    {
        _ballRegistry = ballRegistry;
    }

    public void Initialize(VisualTreeAsset healthBarTemplate, BattleUISettings UIsettings, UIDocument uiDoc)
    {
        _healthBarTemplate = healthBarTemplate;
        _UIsettings = UIsettings;
        _root = uiDoc.rootVisualElement;
        _barsContainer = _root.Q<VisualElement>("HealthBarsContainer");

        BuildTeams();
        RegisterCallback();
    }

    public void SetTeamHealth(SideConfig side)
    {
        VisualElement healthBar = _healthBarTemplate.CloneTree();
        _barsContainer.Add(healthBar);

        var team = _teams[side];
        team.Bar = healthBar;

        UpdateTeamBar(side);
    }

    public void UpdateHealthBar(int current, int max, VisualElement fill, Label text)
    {
        text.text = $"{current}/{max}";
        fill.style.width = Length.Percent((float)current / max * 100);
    }

    public void UpdateBarsAlignment()
    {
        int count = _teams.Count;
        if (count == 0) return;

        _barsContainer.style.justifyContent = Justify.SpaceBetween;
        float targetWidth = _UIsettings.BaseBarWidth;

        if (count == 1)
        {
            _barsContainer.style.justifyContent = Justify.Center;
            targetWidth = _UIsettings.BigBarWidth;
        }
        else if (count == 2)
        {
            targetWidth = _UIsettings.MediumBarWidth;
        }

        foreach (var kvp in _teams)
        {
            SideConfig side = kvp.Key;
            var team = kvp.Value;

            var container = team.Bar.Q<VisualElement>("HealthContainer");
            var flag = team.Bar.Q<Image>("CountryFlag");

            flag.sprite = side.Flag;
            container.style.width = targetWidth;
        }
    }

    private void BuildTeams()
    {
        var grouped = _ballRegistry.GetAllBalls().GroupBy(b => b.GetSide);

        foreach (var group in grouped)
        {
            var side = group.Key;
            var balls = group.ToList();

            var team = new TeamData
            {
                Balls = balls,
                MaxHealth = balls.Sum(b => b.Health.GetMaxHealth),
                CurrentHealth = balls.Sum(b => b.Health.GetCurrentHealth)
            };

            _teams[side] = team;

            foreach (var ball in balls)
            {
                Action<int> damageHandler = (_) => UpdateTeamBar(side);
                _damageHandlers[ball] = damageHandler;
                ball.Health.OnDamaged += damageHandler;

                Action destroyHandler = () =>
                {
                    team.Balls.Remove(ball);
                    UpdateTeamBar(side);
                };
                _destroyHandlers[ball] = destroyHandler;
                ball.OnDestroyed += destroyHandler;
            }
        }
    }

    private void UpdateTeamBar(SideConfig side)
    {
        var team = _teams[side];
        if (team.Bar == null) return;

        team.CurrentHealth = team.Balls
            .Where(b => b != null && b.gameObject != null)
            .Sum(b => b.Health.GetCurrentHealth);

        var fill = team.Bar.Q<VisualElement>("HealthFill");
        var text = team.Bar.Q<Label>("HealthText");

        UpdateHealthBar(team.CurrentHealth, team.MaxHealth, fill, text);
    }

    private void RegisterCallback()
    {
        _barsContainer.RegisterCallback<GeometryChangedEvent>(evt =>
        {
            foreach (var team in _teams.Values)
            {
                if (team.Bar == null) continue;
                if (float.IsNaN(team.Bar.layout.width) || team.Bar.layout.width <= 0)
                    return;
            }

            UpdateBarsAlignment();
        });
    }

    public void Clear()
    {
        foreach (var kvp in _damageHandlers)
        {
            if (kvp.Key != null)
                kvp.Key.Health.OnDamaged -= kvp.Value;
        }
        _damageHandlers.Clear();

        foreach (var kvp in _destroyHandlers)
        {
            if (kvp.Key != null)
                kvp.Key.OnDestroyed -= kvp.Value;
        }
        _destroyHandlers.Clear();

        _teams.Clear();
    }
}
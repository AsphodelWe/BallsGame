using System.Collections.Generic;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;

public class IndividualHealthUI : IUiStrategy
{
    private VisualTreeAsset _healthBarTemplate;
    private BattleUISettings _UIsettings;
    private VisualElement _root;
    private VisualElement _barsContainer;
    private Dictionary<BallPresent, VisualElement> _activeBars = new();

    public IndividualHealthUI() { }

    public void Initialize(VisualTreeAsset healthBarTemplate, BattleUISettings UIsettings, UIDocument uiDoc)
    {
        ;
        _healthBarTemplate = healthBarTemplate;
        _UIsettings = UIsettings;
        _root = uiDoc.rootVisualElement;
        _barsContainer = _root.Q<VisualElement>("HealthBarsContainer");

        RegisterCallback();
    }

    public void UpdateHealthBar(int current, int max, VisualElement fill, Label text)
    {
        text.text = $"{current}/{max}";
        fill.style.width = Length.Percent((float)current / max * 100);
    }

    public void SetBallHealth(BallPresent ball)
    {
        VisualElement healthBar = _healthBarTemplate.CloneTree();
        _barsContainer.Add(healthBar);
        _activeBars[ball] = healthBar;

        var fill = healthBar.Q<VisualElement>("HealthFill");
        var text = healthBar.Q<Label>("HealthText");

        UpdateHealthBar(ball.Health.GetCurrentHealth, ball.Health.GetMaxHealth, fill, text);

        ball.Health.OnHealthChanged += (current, max) =>
            UpdateHealthBar(current, max, fill, text);

    }

    public void UpdateBarsAlignment()
    {
        int count = _activeBars.Count;
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

        foreach (var kvp in _activeBars)
        {
            BallPresent ball = kvp.Key;
            VisualElement bar = kvp.Value;
            var container = bar.Q<VisualElement>("HealthContainer");
            var flag = bar.Q<Image>("CountryFlag");

            flag.sprite = ball.GetSprite;
            container.style.width = targetWidth;
        }
    }

    private void RegisterCallback()
    {
        _barsContainer.RegisterCallback<GeometryChangedEvent>(evt =>
       {
           foreach (var bar in _activeBars.Values)
           {
               if (float.IsNaN(bar.layout.width) || bar.layout.width <= 0)
                   return;
           }

           UpdateBarsAlignment();
       });
    }


}

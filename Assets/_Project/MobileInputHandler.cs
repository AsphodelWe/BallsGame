using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class MobileInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private UIDocument _mobileUI;
    public event Action OnBattleRestart;
    public event Action OnExitToMenu;

    public void Enable()
    {
        var root = _mobileUI.rootVisualElement;
        var container = root.Q<VisualElement>("MobileInputContainer");
        container.style.display = DisplayStyle.Flex;

        root.Q<Button>("MobileRestartButton").clicked += () => OnBattleRestart?.Invoke();
        root.Q<Button>("MobileExitButton").clicked += () => OnExitToMenu?.Invoke();
    }

    public void Disable() { }
}

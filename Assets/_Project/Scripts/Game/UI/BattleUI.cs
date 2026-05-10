using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleUI : MonoBehaviour
{
    public event Action OnBattleStarted;
    private UIDocument _document;
    private Button _startBattleButton;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        if (_document == null)
        {
            Debug.LogError("UIDocument component missing on BattleUI!");
            return;
        }

        var root = _document.rootVisualElement;
        _startBattleButton = root.Q<Button>("StartButton");

        if (_startBattleButton == null)
        {
            Debug.LogError("StartButton not found in UXML!");
        }
    }

    private void OnEnable()
    {
        if (_startBattleButton != null)
            _startBattleButton.clicked += OnStartButtonClicked;
    }

    private void OnDisable()
    {
        if (_startBattleButton != null)
            _startBattleButton.clicked -= OnStartButtonClicked;
    }

    private void OnStartButtonClicked()
    {
        SetStartButtonVisible(false);

        SetCursorVisible(false);

        OnBattleStarted?.Invoke();
    }

    public void SetStartButtonVisible(bool visible)
    {
        if (_startBattleButton != null)
            _startBattleButton.visible = visible;
    }

    public void SetCursorVisible(bool visible)
    {
        UnityEngine.Cursor.visible = visible;
        if (!visible)
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        else
            UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    public void ShowStartButton() => SetStartButtonVisible(true);
    public void HideStartButton() => SetStartButtonVisible(false);
    public void ShowCursor() => SetCursorVisible(true);
    public void HideCursor() => SetCursorVisible(false);
}
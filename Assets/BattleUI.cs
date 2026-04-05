using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BattleUI : MonoBehaviour
{
    private UIDocument _document;
    private Button _startBattleButton;

    public void Initialize()
    {
        _document = GetComponent<UIDocument>();
        var root = _document.rootVisualElement;

        _startBattleButton = root.Q<Button>("StartButton");
        _startBattleButton.clicked += StartBattle;
    }

    public void StartBattle()
    {
        Time.timeScale = 1;
        _startBattleButton.visible = false;
    }

    public void SetPause()
    {
        Time.timeScale = 0;
    }

    public void SetVisualStartBattleButton()
    {
        _startBattleButton.visible = true;
    }
}
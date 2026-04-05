using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputHandlerBattle : MonoBehaviour
{
    public event Action OnBattleRestart;
    public event Action OnExitToMenu;
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartLevel();
        }

        if (Keyboard.current.leftCtrlKey.isPressed && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Time.timeScale = 1;
            ExitToMenu();
        }
    }

    private void RestartLevel()
    {
        Time.timeScale = 1;
        OnBattleRestart?.Invoke();
    }

    private void ExitToMenu()
    {
        Time.timeScale = 1;
        OnExitToMenu?.Invoke();
    }
}

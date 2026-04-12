using System;
using UnityEngine;

public interface IInputHandler
{
    event Action OnBattleRestart;
    event Action OnExitToMenu;
    void Enable();
    void Disable();
}

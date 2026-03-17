using System;
using UnityEngine;

public class BallView : MonoBehaviour
{
    private Sprite _flag;
    public void Initialize(Sprite flag)
    {
        _flag = flag;
    }
    public Sprite GetFlag => _flag;
}

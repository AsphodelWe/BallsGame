using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SideConfig", menuName = "Scripts/Scriptable Objects/SideConfig")]
public class SideConfig : ScriptableObject
{
    public string SideName;
    public SideConfig EnemySide;
}

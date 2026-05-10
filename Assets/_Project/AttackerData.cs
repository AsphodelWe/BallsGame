using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackerData", menuName = "Scriptable Objects/AttackerData")]
public class AttackerData : ScriptableObject
{
    public List<AttackerConfig> AttackerList = new();
}

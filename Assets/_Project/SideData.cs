using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SideData", menuName = "Scriptable Objects/SideData")]
public class SideData : ScriptableObject
{
    public List<SideConfig> SideInfo = new();
}

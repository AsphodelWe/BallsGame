using System;
using UnityEngine;

public abstract class Attacker : MonoBehaviour
{
    public int Damage { get; set; }
    public abstract ITargetStrategy TargetStrategy { get; set; }
    public SideConfig Side {get;set;}
}

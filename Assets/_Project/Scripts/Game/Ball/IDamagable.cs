using System;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(int damage, Vector2 hitDirection);

    SideConfig GetSide{get;}
}

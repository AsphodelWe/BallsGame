using UnityEngine;

public abstract class PlacementStrategy
{
    public abstract Ghost HandlePostPlacement(Ghost placedGhost, GhostFactory factory, IUnitPlacementView view);
}

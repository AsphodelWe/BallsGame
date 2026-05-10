using UnityEngine;

public class PCPlacementStrategy: PlacementStrategy
{
    public override Ghost HandlePostPlacement(Ghost placedGhost, GhostFactory factory, IUnitPlacementView view)
    {
        // Создаем нового призрака "про запас"
        var newGhost = factory.Create(placedGhost.Country);
        var mousePos = view.GetWorldPosition();

        return newGhost;
    }
}

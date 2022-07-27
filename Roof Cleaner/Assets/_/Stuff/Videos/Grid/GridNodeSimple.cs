using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNodeSimple {

    private Grid<GridNodeSimple> grid;
    private int x;
    private int y;

    public GridNodeSimple(Grid<GridNodeSimple> grid, int x, int y) {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void DrawDebugQuadrant() {
        Vector3 worldPos00 = grid.GetWorldPosition(x, y);
        Vector3 worldPos10 = grid.GetWorldPosition(x + 1, y);
        Vector3 worldPos01 = grid.GetWorldPosition(x, y + 1);
        Vector3 worldPos11 = grid.GetWorldPosition(x + 1, y + 1);

        Debug.DrawLine(worldPos00, worldPos01, Color.white, 999f);
        Debug.DrawLine(worldPos00, worldPos10, Color.white, 999f);
        Debug.DrawLine(worldPos01, worldPos11, Color.white, 999f);
        Debug.DrawLine(worldPos10, worldPos11, Color.white, 999f);
    }
        
}
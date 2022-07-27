/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class GridPrefabVisual : MonoBehaviour {

    public static GridPrefabVisual Instance { get; private set; }

    [SerializeField] private Transform pfGridPrefabVisualNode = null;

    private List<Transform> visualNodeList;
    private Transform[,] visualNodeArray;
    private Grid<GridPrefabVisualObject> grid;
    private bool updateVisual;

    private void Awake() {
        Instance = this;
        visualNodeList = new List<Transform>();
    }

    private void Update() {
        if (updateVisual) {
            updateVisual = false;
            UpdateVisual(grid);
        }
    }

    public void Setup(Grid<GridPrefabVisualObject> grid) {
        this.grid = grid;
        visualNodeArray = new Transform[grid.GetWidth(), grid.GetHeight()];

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                Vector3 gridPosition = new Vector3(x, y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f;
                Transform visualNode = CreateVisualNode(gridPosition);
                visualNodeArray[x, y] = visualNode;
                visualNodeList.Add(visualNode);
            }
        }

        HideNodeVisuals();

        grid.OnGridObjectChanged += Grid_OnGridObjectChanged;
    }

    private void Grid_OnGridObjectChanged(object sender, Grid<GridPrefabVisualObject>.OnGridObjectChangedEventArgs e) {
        updateVisual = true;
    }

    public void UpdateVisual(Grid<GridPrefabVisualObject> grid) {
        HideNodeVisuals();

        for (int x = 0; x < grid.GetWidth(); x++) {
            for (int y = 0; y < grid.GetHeight(); y++) {
                GridPrefabVisualObject gridObject = grid.GetGridObject(x, y);
                
                Transform visualNode = visualNodeArray[x, y];
                visualNode.gameObject.SetActive(true);
                SetupVisualNode(visualNode);
            }
        }
    }

    private void HideNodeVisuals() {
        foreach (Transform visualNodeTransform in visualNodeList) {
            visualNodeTransform.gameObject.SetActive(false);
        }
    }

    private Transform CreateVisualNode(Vector3 position) {
        Transform visualNodeTransform = Instantiate(pfGridPrefabVisualNode, position, Quaternion.identity);
        return visualNodeTransform;
    }

    private void SetupVisualNode(Transform visualNodeTransform) {
    }
    
    /*
     * Represents a single Grid Object
     * */
    public class GridPrefabVisualObject {

        private Grid<GridPrefabVisualObject> grid;
        private int x;
        private int y;
        private int value;

        public GridPrefabVisualObject(Grid<GridPrefabVisualObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetValue(int value) {
            this.value = value;
            grid.TriggerGridObjectChanged(x, y);
        }

        public override string ToString() {
            return x + "," + y + "\n" + value.ToString();
        }

    }
}


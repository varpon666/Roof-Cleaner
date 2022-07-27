using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

namespace CodeMonkey.GridBuildingSystemVideo {

    public class GridBuildingSystem2D : MonoBehaviour {

        public static GridBuildingSystem2D Instance { get; private set; }

        public event EventHandler OnSelectedChanged;
        public event EventHandler OnObjectPlaced;


        private Grid<GridObject> grid;
        [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
        private PlacedObjectTypeSO placedObjectTypeSO;
        private PlacedObjectTypeSO.Dir dir;

        private void Awake() {
            Instance = this;

            int gridWidth = 10;
            int gridHeight = 10;
            float cellSize = 10f;
            grid = new Grid<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));

            placedObjectTypeSO = null;
        }

        public class GridObject {

            private Grid<GridObject> grid;
            private int x;
            private int y;
            public PlacedObject placedObject;

            public GridObject(Grid<GridObject> grid, int x, int y) {
                this.grid = grid;
                this.x = x;
                this.y = y;
                placedObject = null;
            }

            public override string ToString() {
                return x + ", " + y + "\n" + placedObject;
            }

            public void SetPlacedObject(PlacedObject placedObject) {
                this.placedObject = placedObject;
                grid.TriggerGridObjectChanged(x, y);
            }

            public void ClearPlacedObject() {
                placedObject = null;
                grid.TriggerGridObjectChanged(x, y);
            }

            public PlacedObject GetPlacedObject() {
                return placedObject;
            }

            public bool CanBuild() {
                return placedObject == null;
            }

        }

        private void Update() {
            if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null) {
                Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
                grid.GetXY(mousePosition, out int x, out int z);

                Vector2Int placedObjectOrigin = new Vector2Int(x, z);

                // Test Can Build
                List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
                bool canBuild = true;
                foreach (Vector2Int gridPosition in gridPositionList) {
                    if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                        canBuild = false;
                        break;
                    }
                }

                if (canBuild) {
                    Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                    Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

                    PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);
                    placedObject.transform.rotation = Quaternion.Euler(0, 0, -placedObjectTypeSO.GetRotationAngle(dir));

                    foreach (Vector2Int gridPosition in gridPositionList) {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    }

                    OnObjectPlaced?.Invoke(this, EventArgs.Empty);

                    //DeselectObjectType();
                } else {
                    // Cannot build here
                    UtilsClass.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
                }
            }

            if (Input.GetKeyDown(KeyCode.R)) {
                dir = PlacedObjectTypeSO.GetNextDir(dir);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectTypeSO = placedObjectTypeSOList[0]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectTypeSO = placedObjectTypeSOList[1]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { placedObjectTypeSO = placedObjectTypeSOList[2]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { placedObjectTypeSO = placedObjectTypeSOList[3]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { placedObjectTypeSO = placedObjectTypeSOList[4]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { placedObjectTypeSO = placedObjectTypeSOList[5]; RefreshSelectedObjectType(); }

            if (Input.GetKeyDown(KeyCode.Alpha0)) { DeselectObjectType(); }


            if (Input.GetMouseButtonDown(1)) {
                Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
                PlacedObject placedObject = grid.GetGridObject(mousePosition).GetPlacedObject();
                if (placedObject != null) {
                    // Demolish
                    placedObject.DestroySelf();

                    List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();
                    foreach (Vector2Int gridPosition in gridPositionList) {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                    }
                }
            }
        }

        private void DeselectObjectType() {
            placedObjectTypeSO = null; RefreshSelectedObjectType();
        }

        private void RefreshSelectedObjectType() {
            OnSelectedChanged?.Invoke(this, EventArgs.Empty);
        }


        public Vector2Int GetGridPosition(Vector3 worldPosition) {
            grid.GetXY(worldPosition, out int x, out int z);
            return new Vector2Int(x, z);
        }

        public Vector3 GetMouseWorldSnappedPosition() {
            Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();
            grid.GetXY(mousePosition, out int x, out int y);

            if (placedObjectTypeSO != null) {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();
                return placedObjectWorldPosition;
            } else {
                return mousePosition;
            }
        }

        public Quaternion GetPlacedObjectRotation() {
            if (placedObjectTypeSO != null) {
                return Quaternion.Euler(0, 0, -placedObjectTypeSO.GetRotationAngle(dir));
            } else {
                return Quaternion.identity;
            }
        }

        public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
            return placedObjectTypeSO;
        }

    }

}
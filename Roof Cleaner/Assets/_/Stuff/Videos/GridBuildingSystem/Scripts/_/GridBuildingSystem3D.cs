using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

namespace CodeMonkey.GridBuildingSystemVideo {

    public class GridBuildingSystem3D : MonoBehaviour {

        public static GridBuildingSystem3D Instance { get; private set; }

        public event EventHandler OnSelectedChanged;
        public event EventHandler OnObjectPlaced;


        private GridXZ<GridObject> grid;
        [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
        private PlacedObjectTypeSO placedObjectTypeSO;
        private PlacedObjectTypeSO.Dir dir;

        private bool isDemolishActive;

        private void Awake() {
            Instance = this;

            int gridWidth = 10;
            int gridHeight = 10;
            float cellSize = 10f;
            grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(0, 0, 0), (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));

            placedObjectTypeSO = null;
        }

        public class GridObject {

            private GridXZ<GridObject> grid;
            private int x;
            private int y;
            public PlacedObject placedObject;

            public GridObject(GridXZ<GridObject> grid, int x, int y) {
                this.grid = grid;
                this.x = x;
                this.y = y;
                placedObject = null;
            }

            public override string ToString() {
                return x + ", " + y + "\n" + placedObject;
            }

            public void TriggerGridObjectChanged() {
                grid.TriggerGridObjectChanged(x, y);
            }

            public void SetPlacedObject(PlacedObject placedObject) {
                this.placedObject = placedObject;
                TriggerGridObjectChanged();
            }

            public void ClearPlacedObject() {
                placedObject = null;
                TriggerGridObjectChanged();
            }

            public PlacedObject GetPlacedObject() {
                return placedObject;
            }

            public bool CanBuild() {
                return placedObject == null;
            }

        }

        private void Update() {
            HandleTypeSelect();
            HandleNormalObjectPlacement();
            HandleDirRotation();
            HandleDemolish();

            if (Input.GetMouseButtonDown(1)) {
                DeselectObjectType();
            }
        }

        private void HandleNormalObjectPlacement() {
            if (Input.GetMouseButtonDown(0) && placedObjectTypeSO != null && !UtilsClass.IsPointerOverUI()) {
                Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
                grid.GetXZ(mousePosition, out int x, out int z);

                Vector2Int placedObjectOrigin = new Vector2Int(x, z);
                if (TryPlaceObject(placedObjectOrigin, placedObjectTypeSO, dir, out PlacedObject placedObject)) {
                    // Object placed
                } else {
                    // Error!
                    UtilsClass.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
                }
            }
        }

        private void HandleTypeSelect() {
            if (Input.GetKeyDown(KeyCode.Alpha1)) { placedObjectTypeSO = placedObjectTypeSOList[0]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { placedObjectTypeSO = placedObjectTypeSOList[1]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { placedObjectTypeSO = placedObjectTypeSOList[2]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha4)) { placedObjectTypeSO = placedObjectTypeSOList[3]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha5)) { placedObjectTypeSO = placedObjectTypeSOList[4]; RefreshSelectedObjectType(); }
            if (Input.GetKeyDown(KeyCode.Alpha6)) { placedObjectTypeSO = placedObjectTypeSOList[5]; RefreshSelectedObjectType(); }

            if (Input.GetKeyDown(KeyCode.Alpha0)) { DeselectObjectType(); }
        }

        private void HandleDirRotation() {
            if (Input.GetKeyDown(KeyCode.R)) {
                dir = PlacedObjectTypeSO.GetNextDir(dir);
            }
        }

        private void HandleDemolish() {
            if (isDemolishActive && Input.GetMouseButtonDown(0) && !UtilsClass.IsPointerOverUI()) {
                Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
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
            placedObjectTypeSO = null;
            isDemolishActive = false;
            RefreshSelectedObjectType();
        }

        private void RefreshSelectedObjectType() {
            //UpdateCanBuildTilemap();

            if (placedObjectTypeSO == null) {
                //TilemapVisual.Instance.Hide();
            } else {
                //TilemapVisual.Instance.Show();
            }

            OnSelectedChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateCanBuildTilemap() {
            /*
            // Not implemented by default
            for (int x = 0; x < grid.GetWidth(); x++) {
                for (int y = 0; y < grid.GetHeight(); y++) {
                    // Tilemap
                    Tilemap.Instance.SetTilemapSprite(new Vector3(x, y),
                        grid.GetGridObject(x, y).CanBuild() ?
                        Tilemap.TilemapObject.TilemapSprite.CanBuild :
                        Tilemap.TilemapObject.TilemapSprite.CannotBuild);
                }
            }*/
        }

        public bool TryPlaceObject(int x, int y, PlacedObjectTypeSO placedObjectTypeSO, PlacedObjectTypeSO.Dir dir) {
            return TryPlaceObject(new Vector2Int(x, y), placedObjectTypeSO, dir, out PlacedObject placedObject);
        }

        public bool TryPlaceObject(Vector2Int placedObjectOrigin, PlacedObjectTypeSO placedObjectTypeSO, PlacedObjectTypeSO.Dir dir) {
            return TryPlaceObject(placedObjectOrigin, placedObjectTypeSO, dir, out PlacedObject placedObject);
        }

        public bool TryPlaceObject(Vector2Int placedObjectOrigin, PlacedObjectTypeSO placedObjectTypeSO, PlacedObjectTypeSO.Dir dir, out PlacedObject placedObject) {
            // Test Can Build
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList) {
                //bool isValidPosition = grid.IsValidGridPositionWithPadding(gridPosition);
                bool isValidPosition = grid.IsValidGridPosition(gridPosition);
                if (!isValidPosition) {
                    // Not valid
                    canBuild = false;
                    break;
                }
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild()) {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild) {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                placedObject = PlacedObject.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);

                foreach (Vector2Int gridPosition in gridPositionList) {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }

                placedObject.GridSetupDone();

                OnObjectPlaced?.Invoke(placedObject, EventArgs.Empty);

                return true;
            } else {
                // Cannot build here
                placedObject = null;
                return false;
            }
        }


        public Vector2Int GetGridPosition(Vector3 worldPosition) {
            grid.GetXZ(worldPosition, out int x, out int z);
            return new Vector2Int(x, z);
        }

        public Vector3 GetWorldPosition(Vector2Int gridPosition) {
            return grid.GetWorldPosition(gridPosition.x, gridPosition.y);
        }

        public GridObject GetGridObject(Vector2Int gridPosition) {
            return grid.GetGridObject(gridPosition.x, gridPosition.y);
        }

        public GridObject GetGridObject(Vector3 worldPosition) {
            return grid.GetGridObject(worldPosition);
        }

        public bool IsValidGridPosition(Vector2Int gridPosition) {
            return grid.IsValidGridPosition(gridPosition);
        }

        public Vector3 GetMouseWorldSnappedPosition() {
            Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
            grid.GetXZ(mousePosition, out int x, out int z);

            if (placedObjectTypeSO != null) {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
                return placedObjectWorldPosition;
            } else {
                return mousePosition;
            }
        }

        public Quaternion GetPlacedObjectRotation() {
            if (placedObjectTypeSO != null) {
                return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
            } else {
                return Quaternion.identity;
            }
        }

        public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
            return placedObjectTypeSO;
        }

        public void SetSelectedPlacedObject(PlacedObjectTypeSO placedObjectTypeSO) {
            this.placedObjectTypeSO = placedObjectTypeSO;
            isDemolishActive = false;
            RefreshSelectedObjectType();
        }

        public void SetDemolishActive() {
            placedObjectTypeSO = null;
            isDemolishActive = true;
            RefreshSelectedObjectType();
        }

        public bool IsDemolishActive() {
            return isDemolishActive;
        }

    }

}
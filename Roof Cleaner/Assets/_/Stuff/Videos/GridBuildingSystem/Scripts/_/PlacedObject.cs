using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey.GridBuildingSystemVideo {

    public class PlacedObject : MonoBehaviour {

        public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, PlacedObjectTypeSO.Dir dir, PlacedObjectTypeSO placedObjectTypeSO) {
            Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

            PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();
            placedObject.placedObjectTypeSO = placedObjectTypeSO;
            placedObject.origin = origin;
            placedObject.dir = dir;

            placedObject.Setup();

            return placedObject;
        }




        private PlacedObjectTypeSO placedObjectTypeSO;
        private Vector2Int origin;
        private PlacedObjectTypeSO.Dir dir;

        protected virtual void Setup() {
            //Debug.Log("PlacedObject.Setup() " + transform);
        }

        public virtual void GridSetupDone() {
            //Debug.Log("PlacedObject.GridSetupDone() " + transform);
        }

        protected virtual void TriggerGridObjectChanged() {
            foreach (Vector2Int gridPosition in GetGridPositionList()) {
                GridBuildingSystem3D.Instance.GetGridObject(gridPosition).TriggerGridObjectChanged();
            }
        }

        public Vector2Int GetGridPosition() {
            return origin;
        }

        public List<Vector2Int> GetGridPositionList() {
            return placedObjectTypeSO.GetGridPositionList(origin, dir);
        }

        public void DestroySelf() {
            Destroy(gameObject);
        }

        public override string ToString() {
            return placedObjectTypeSO.nameString;
        }

    }

}
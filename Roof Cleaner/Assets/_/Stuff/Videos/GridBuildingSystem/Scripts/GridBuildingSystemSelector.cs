using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;

namespace CodeMonkey.GridBuildingSystemVideo {

    public class GridBuildingSystemSelector : MonoBehaviour {


        private void Update() {
            HandleBuildingSelection();
        }

        private void HandleBuildingSelection() {
            if (Input.GetMouseButtonDown(0) && !UtilsClass.IsPointerOverUI() && GridBuildingSystem3D.Instance.GetPlacedObjectTypeSO() == null) {
                // Not building anything
                if (GridBuildingSystem3D.Instance.GetGridObject(Mouse3D.GetMouseWorldPosition()) != null) {
                    PlacedObject placedObject = GridBuildingSystem3D.Instance.GetGridObject(Mouse3D.GetMouseWorldPosition()).GetPlacedObject();
                    if (placedObject != null) {
                        // Clicked on something, Show UI
                        UtilsClass.CreateWorldTextPopup("Cannot Build Here!", Mouse3D.GetMouseWorldPosition());
                        Debug.Log("Clicked on " + placedObject);
                        /*if (placedObject is Smelter) {
                            SmelterUI.Instance.Show(placedObject as Smelter);
                        }*/
                    }
                }
            }
        }

    }

}
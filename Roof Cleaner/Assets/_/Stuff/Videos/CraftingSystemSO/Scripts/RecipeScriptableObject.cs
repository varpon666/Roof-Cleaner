using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeMonkey.CraftingSystem {

    [CreateAssetMenu(menuName = "ScriptableObjects/RecipeScriptableObject")]
    public class RecipeScriptableObject : ScriptableObject {

        public InventoryItemScriptableObject output;

        public InventoryItemScriptableObject item_00;
        public InventoryItemScriptableObject item_10;
        public InventoryItemScriptableObject item_20;

        public InventoryItemScriptableObject item_01;
        public InventoryItemScriptableObject item_11;
        public InventoryItemScriptableObject item_21;

        public InventoryItemScriptableObject item_02;
        public InventoryItemScriptableObject item_12;
        public InventoryItemScriptableObject item_22;


        public InventoryItemScriptableObject GetItem(int x, int y) {
            if (x == 0 && y == 0) return item_00;
            if (x == 1 && y == 0) return item_10;
            if (x == 2 && y == 0) return item_20;

            if (x == 0 && y == 1) return item_01;
            if (x == 1 && y == 1) return item_11;
            if (x == 2 && y == 1) return item_21;

            if (x == 0 && y == 2) return item_02;
            if (x == 1 && y == 2) return item_12;
            if (x == 2 && y == 2) return item_22;

            return null;
        }


    }

}
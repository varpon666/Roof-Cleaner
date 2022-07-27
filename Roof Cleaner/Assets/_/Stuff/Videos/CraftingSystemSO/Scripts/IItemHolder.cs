using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.InventorySystem;

namespace CodeMonkey.CraftingSystem {

    public interface IItemHolder {

        void RemoveItem(Item item);
        void AddItem(Item item);
        bool CanAddItem();

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;
using CodeMonkey.Utils;

namespace CodeMonkey.InventorySystem {

    public class ItemWorld : MonoBehaviour {

        public static ItemWorld SpawnItemWorld(Vector3 position, Item item) {
            Transform transform = Instantiate(ItemAssets.Instance.pfItemWorld, position, Quaternion.identity);

            ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
            itemWorld.SetItem(item);

            return itemWorld;
        }

        public static ItemWorld DropItem(Vector3 dropPosition, Item item) {
            Vector3 randomDir = UtilsClass.GetRandomDir();
            ItemWorld itemWorld = SpawnItemWorld(dropPosition + randomDir * 8f, item);
            itemWorld.GetComponent<Rigidbody2D>().AddForce(randomDir * 40f, ForceMode2D.Impulse);
            return itemWorld;
        }


        private Item item;
        private SpriteRenderer spriteRenderer;
        private UnityEngine.Rendering.Universal.Light2D light2D;
        private TextMeshPro textMeshPro;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            light2D = transform.Find("Light").GetComponent<UnityEngine.Rendering.Universal.Light2D>();
            textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
        }

        public void SetItem(Item item) {
            this.item = item;
            spriteRenderer.sprite = item.GetSprite();
            light2D.color = item.GetColor();
            if (item.amount > 1) {
                textMeshPro.SetText(item.amount.ToString());
            } else {
                textMeshPro.SetText("");
            }
        }

        public Item GetItem() {
            return item;
        }

        public void DestroySelf() {
            Destroy(gameObject);
        }

    }

}
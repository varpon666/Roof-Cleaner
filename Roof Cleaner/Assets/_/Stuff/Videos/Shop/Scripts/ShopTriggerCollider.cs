/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTriggerCollider : MonoBehaviour {

    [SerializeField] private UI_Shop uiShop = null;

    private void OnTriggerEnter2D(Collider2D collider) {
        IShopCustomer shopCustomer = collider.GetComponent<IShopCustomer>();
        if (shopCustomer != null) {
            uiShop.Show(shopCustomer);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
        IShopCustomer shopCustomer = collider.GetComponent<IShopCustomer>();
        if (shopCustomer != null) {
            uiShop.Hide();
        }
    }

}

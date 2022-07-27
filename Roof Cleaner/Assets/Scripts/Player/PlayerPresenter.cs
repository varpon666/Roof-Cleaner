using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Dirt>(out Dirt dirt))
        {
            dirt.RemoveDirt(dirt);
        }
    }
}

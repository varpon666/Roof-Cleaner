using System;
using UnityEngine;

public class Dirt : MonoBehaviour
{
    public Action<Dirt> OnRemoveDirt;

    public void RemoveDirt(Dirt dirt)
    {
        OnRemoveDirt?.Invoke(dirt);
    }
}

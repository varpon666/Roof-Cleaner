using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceSlider : MonoBehaviour
{
    private Vector3 _normal;

    public Vector3 _border { private set; get; }
    public Vector3 _borderMin { private set; get; }
    public Vector3 _borderMax { private set; get; }

    public float _offsetX { private set; get; }
    public float _offsetY { private set; get; }
    public float _offsetZ { private set; get; }

    private void OnCollisionEnter(Collision collision)
    {
        _normal = collision.contacts[0].normal;

        _border = collision.collider.bounds.size;
        _borderMax = collision.collider.bounds.max;
        _borderMin = collision.collider.bounds.min;

        _offsetX = (4 * _border.x) / 100;
        _offsetY = (1 * _borderMax.y) / 100;
        _offsetZ = (1 * _borderMax.z) / 100;
    }

    public Vector3 Project(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _normal);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private Vector3 _lastMouseFramePosition;
    private Vector3 _moveFactorX;

    public Vector3 MoveFactor => _moveFactorX;

    private void Update()
    {
        if (GameLoop.CanPlay == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastMouseFramePosition = Input.mousePosition;

                GameLoop.CanMovePlayer = true;
            }
            else if (Input.GetMouseButton(0))
            {
                _moveFactorX = Input.mousePosition - _lastMouseFramePosition;

                _lastMouseFramePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _moveFactorX = Vector3.zero;

                GameLoop.CanMovePlayer = false;
            }
        }
    }
}

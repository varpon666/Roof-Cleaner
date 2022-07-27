using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDriverPlayer : MonoBehaviour {

    private CarDriver carDriver;

    private void Awake() {
        carDriver = GetComponent<CarDriver>();
    }

    private void Update() {
        float forwardAmount = Input.GetAxisRaw("Vertical");
        float turnAmount = Input.GetAxisRaw("Horizontal");
        carDriver.SetInputs(forwardAmount, turnAmount);
    }

}

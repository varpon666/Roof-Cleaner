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
using CodeMonkey.Utils;

public class MoveRoam_ChatBubble : MonoBehaviour {

    private Vector3 startPosition;
    private Vector3 targetMovePosition;
    private State state;
    private float waitTimer;

    private enum State {
        Moving,
        Waiting
    }

    private void Awake() {
        startPosition = transform.position;
        state = State.Waiting;
        waitTimer = Random.Range(0f, 2f);
    }

    private void Start() {
        SetRandomMovePosition();
    }

    private void SetRandomMovePosition() {
        targetMovePosition = startPosition + UtilsClass.GetRandomDir() * Random.Range(0f, 50f);
    }

    private void Update() {
        switch (state) {
            default:
            case State.Waiting:
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f) {
                    SetRandomMovePosition();
                    state = State.Moving;
                }
                break;
            case State.Moving:
                SetMovePosition(targetMovePosition);

                float arrivedAtPositionDistance = 1f;
                if (Vector3.Distance(transform.position, targetMovePosition) < arrivedAtPositionDistance) {
                    // Reached position
                    state = State.Waiting;
                    waitTimer = Random.Range(0f, 4f);
                }
                break;
        }
    }

    private void SetMovePosition(Vector3 movePosition) {
        GetComponent<IMovePosition>().SetMovePosition(movePosition, () => { });
    }


}

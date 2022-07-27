using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public static class TimeTickSystem {

    public class OnTickEventArgs : EventArgs {
        public int tick;
    }
    
    public static event EventHandler<OnTickEventArgs> OnTick;
    public static event EventHandler<OnTickEventArgs> OnTick_5;

    private const float TICK_TIMER_MAX = .1f;

    private static GameObject timeTickSystemGameObject;
    private static int tick;

    public static void Create() {
        if (timeTickSystemGameObject == null) {
            timeTickSystemGameObject = new GameObject("TimeTickSystem");
            timeTickSystemGameObject.AddComponent<TimeTickSystemObject>();
        }
    }

    public static int GetTick() {
        return tick;
    }

    private class TimeTickSystemObject : MonoBehaviour {
        
        private float tickTimer;

        private void Awake() {
            tick = 0;
        }

        private void Update() {
            tickTimer += Time.deltaTime;
            if (tickTimer >= TICK_TIMER_MAX) {
                tickTimer -= TICK_TIMER_MAX;
                tick++;
                if (OnTick != null) OnTick(this, new OnTickEventArgs { tick = tick });

                if (tick % 5 == 0) {
                    if (OnTick_5 != null) OnTick_5(this, new OnTickEventArgs { tick = tick });
                }
            }
        }

    }

}

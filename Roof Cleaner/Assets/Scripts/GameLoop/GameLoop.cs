using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    private static bool _canPlay;
    private static bool _canMovePlayer;

    public static bool CanPlay { set { _canPlay = value; } get { return _canPlay; } }
    public static bool CanMovePlayer { set { _canMovePlayer = value; } get { return _canMovePlayer; } }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        _canPlay = true;
    }
}

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
using Minimap;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour {

    [SerializeField] private MinimapIcon playerMinimapIcon = null;

    private void Start() {
        Minimap.Minimap.Init();

        CMDebug.ButtonUI(new Vector2(300, 200), "Show Minimap", Minimap.Minimap.ShowWindow);
        CMDebug.ButtonUI(new Vector2(300, 160), "Hide Minimap", Minimap.Minimap.HideWindow);
        
        CMDebug.ButtonUI(new Vector2(300, 120), "Player Icon Show", playerMinimapIcon.Show);
        CMDebug.ButtonUI(new Vector2(300, 80), "Player Icon Hide", playerMinimapIcon.Hide);
        
        CMDebug.ButtonUI(new Vector2(300, 20), "Zoom In", Minimap.Minimap.ZoomIn);
        CMDebug.ButtonUI(new Vector2(300, -20), "Zoom Out", Minimap.Minimap.ZoomOut);
    }
}

/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading the Code Monkey Utilities
    I hope you find them useful in your projects
    If you have any questions use the contact form
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class RadarChart_Testing : MonoBehaviour {

    [SerializeField] private UI_StatsRadarChart uiStatsRadarChart = null;
    [SerializeField] private UI_TestStatsRadarChart uiTestStatsRadarChart = null;

    private void Start() {
        Stats stats = new Stats(10, 2, 5, 10, 15);

        uiStatsRadarChart.SetStats(stats);
        uiTestStatsRadarChart.SetStats(stats);

        /*
        CMDebug.ButtonUI(new Vector2(100, +20), "ATK++", () => stats.IncreaseStatAmount(Stats.Type.Attack));
        CMDebug.ButtonUI(new Vector2(100, -20), "ATK--", () => stats.DecreaseStatAmount(Stats.Type.Attack));
        
        CMDebug.ButtonUI(new Vector2(180, +20), "DEF++", () => stats.IncreaseStatAmount(Stats.Type.Defence));
        CMDebug.ButtonUI(new Vector2(180, -20), "DEF--", () => stats.DecreaseStatAmount(Stats.Type.Defence));
        
        CMDebug.ButtonUI(new Vector2(260, +20), "SPD++", () => stats.IncreaseStatAmount(Stats.Type.Speed));
        CMDebug.ButtonUI(new Vector2(260, -20), "SPD--", () => stats.DecreaseStatAmount(Stats.Type.Speed));
        
        CMDebug.ButtonUI(new Vector2(340, +20), "MAN++", () => stats.IncreaseStatAmount(Stats.Type.Mana));
        CMDebug.ButtonUI(new Vector2(340, -20), "MAN--", () => stats.DecreaseStatAmount(Stats.Type.Mana));
        
        CMDebug.ButtonUI(new Vector2(420, +20), "HEL++", () => stats.IncreaseStatAmount(Stats.Type.Health));
        CMDebug.ButtonUI(new Vector2(420, -20), "HEL--", () => stats.DecreaseStatAmount(Stats.Type.Health));
        //*/
    }

}

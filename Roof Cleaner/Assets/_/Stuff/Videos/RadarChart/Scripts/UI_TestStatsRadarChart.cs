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
using UnityEngine.UI;
using CodeMonkey.Utils;

public class UI_TestStatsRadarChart : MonoBehaviour {

    [SerializeField] private Material radarMaterial = null;
    private Color baseMaterialColor;
    private Stats stats;

    public void SetStats(Stats stats) {
        this.stats = stats;

        transform.Find("attack").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Attack);
        transform.Find("attack").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Attack);

        transform.Find("defence").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Defence);
        transform.Find("defence").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Defence);

        transform.Find("speed").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Speed);
        transform.Find("speed").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Speed);

        transform.Find("mana").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Mana);
        transform.Find("mana").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Mana);

        transform.Find("health").Find("incBtn").GetComponent<Button_UI>().ClickFunc = () => stats.IncreaseStatAmount(Stats.Type.Health);
        transform.Find("health").Find("decBtn").GetComponent<Button_UI>().ClickFunc = () => stats.DecreaseStatAmount(Stats.Type.Health);

        // Randomize all Stats
        transform.Find("randomBtn").GetComponent<Button_UI>().ClickFunc = () => {
            stats.SetStatAmount(Stats.Type.Attack, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));
            stats.SetStatAmount(Stats.Type.Defence, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));
            stats.SetStatAmount(Stats.Type.Speed, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));
            stats.SetStatAmount(Stats.Type.Mana, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));
            stats.SetStatAmount(Stats.Type.Health, Random.Range(Stats.STAT_MIN, Stats.STAT_MAX));
        };

        // Animate Stats
        bool anim = false;
        FunctionPeriodic.Create(() => {
            if (anim) {
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Attack); else stats.DecreaseStatAmount(Stats.Type.Attack);
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Defence); else stats.DecreaseStatAmount(Stats.Type.Defence);
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Speed); else stats.DecreaseStatAmount(Stats.Type.Speed);
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Mana); else stats.DecreaseStatAmount(Stats.Type.Mana);
                if (Random.Range(0, 100) < 50) stats.IncreaseStatAmount(Stats.Type.Health); else stats.DecreaseStatAmount(Stats.Type.Health);
            }
        }, .1f);

        transform.Find("animBtn").GetComponent<Button_UI>().ClickFunc = () => {
            anim = !anim;
            transform.Find("animBtn").Find("text").GetComponent<Text>().text = "ANIM: " + anim.ToString().ToUpper();
        };

        // Flashing Color
        //baseMaterialColor = radarMaterial.color;
        //baseMaterialColor.g = .8f;
        Color color = radarMaterial.color;
        bool increase = true;
        FunctionUpdater.Create(() => {
            color.g += .3f * Time.deltaTime * ((increase) ? 1f : -1f);
            radarMaterial.color = color;

            if (color.g >= 1f) {
                increase = false;
                //color.g = baseMaterialColor.g;
            }

            if (color.g <= .9f) {
                increase = true;
            }
        });

        stats.OnStatsChanged += Stats_OnStatsChanged;
        UpdateStatsText();
    }

    private void Stats_OnStatsChanged(object sender, System.EventArgs e) {
        UpdateStatsText();
    }

    private void UpdateStatsText() {
        transform.Find("text").GetComponent<Text>().text =
            "ATTACK: " + stats.GetStatAmount(Stats.Type.Attack) + "\n" +
            "DEFENCE: " + stats.GetStatAmount(Stats.Type.Defence) + "\n" +
            "SPEED: " + stats.GetStatAmount(Stats.Type.Speed) + "\n" +
            "MANA: " + stats.GetStatAmount(Stats.Type.Mana) + "\n" +
            "HEALTH: " + stats.GetStatAmount(Stats.Type.Health);
    }

}

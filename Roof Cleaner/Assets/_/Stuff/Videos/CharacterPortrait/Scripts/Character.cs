using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Character {
    
    public event EventHandler OnLeveledUp;
    public event EventHandler OnExperienceGained;

    public Transform transform;
    public string name;

    public int level;
    public int experience;
    public int experienceMax;

    public int STR;
    public int DEX;
    public int CON;
    public int WIS;

    public Character(Transform transform, string name) {
        this.transform = transform;
        this.name = name;
        experience = UnityEngine.Random.Range(0, 100);
        experienceMax = 100;

        level = UnityEngine.Random.Range(0, 10);
        
        STR = UnityEngine.Random.Range(0, 10);
        DEX = UnityEngine.Random.Range(0, 10);
        CON = UnityEngine.Random.Range(0, 10);
        WIS = UnityEngine.Random.Range(0, 10);

        FunctionPeriodic.Create(AddExperience, .025f);
    }

    private void AddExperience() {
        experience++;
        if (experience >= experienceMax) {
            experience = 0;
            level++;
            switch (UnityEngine.Random.Range(0, 4)) {
            case 0: STR++; break;
            case 1: DEX++; break;
            case 2: CON++; break;
            case 3: WIS++; break;
            }
            if (OnLeveledUp != null) OnLeveledUp(this, EventArgs.Empty);
        }
        if (OnExperienceGained != null) OnExperienceGained(this, EventArgs.Empty);
    }
    
    public float GetExperienceNormalized() {
        return experience * 1f / experienceMax;
    }

}

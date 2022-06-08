using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGathering : MonoBehaviour
{
    private FirstPersonController fpsScript;
    public int micesCarried = 0;
    public int miceCollected = 0;
    public int maxCapability;
    void Start()
    {
        fpsScript = GetComponent<FirstPersonController>();
        maxCapability = PlayerPrefs.GetInt("MiceCapability");
        maxCapability = 1; 
    }

    public void addNewMouse()
    {
        micesCarried++;
        fpsScript.MoveSpeed = fpsScript.MoveSpeed * 0.9f;
        miceCollected++;
    }

    public void removeMice()
    {
        micesCarried = 0;
        fpsScript.MoveSpeed = fpsScript.MoveSpeed * (float) Math.Pow(1.11f, micesCarried);
    }
}

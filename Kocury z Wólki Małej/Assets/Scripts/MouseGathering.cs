using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGathering : MonoBehaviour
{
    private FirstPersonController fpsScript;
    private PlayMenu playMenuScript;
    public int micesCarried = 0;
    public int miceCollected = 0;
    public int maxCapability;
    public GameObject playMenuGO;
    void Start()
    {
        fpsScript = GetComponent<FirstPersonController>();
        maxCapability = PlayerPrefs.GetInt("MiceCapability");
        maxCapability = 1;
        playMenuScript = playMenuGO.GetComponent<PlayMenu>();
        playMenuScript.ChangeMiceLeftNumber(maxCapability);
    }

    public void addNewMouse()
    {
        micesCarried++;
        playMenuScript.ChangeMiceCarriedNumber(micesCarried);
        fpsScript.MoveSpeed = fpsScript.MoveSpeed * 0.9f;
        miceCollected++;
    }

    public void removeMice()
    {
        micesCarried = 0;
        playMenuScript.ChangeMiceCarriedNumber(micesCarried);
        playMenuScript.ChangeMiceLeftNumber(maxCapability - miceCollected);
        fpsScript.MoveSpeed = fpsScript.MoveSpeed * (float) Math.Pow(1.11f, micesCarried);
    }
}

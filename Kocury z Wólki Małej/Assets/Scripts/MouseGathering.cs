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
    public int miceToCollect;
    public GameObject playMenuGO;
    void Start()
    {
        fpsScript = GetComponent<FirstPersonController>();
        maxCapability = PlayerPrefs.GetInt("MiceCapability");
        miceToCollect = PlayerPrefs.GetInt("MiceToCollect");
        // miceToCollect = 1;
        // maxCapability = 4;
        playMenuScript = playMenuGO.GetComponent<PlayMenu>();
        playMenuScript.ChangeMiceCarriedNumber(0, maxCapability);
        playMenuScript.ChangeMiceLeftNumber(miceToCollect, miceToCollect);
    }

    public void addNewMouse()
    {
        if (micesCarried < maxCapability)
        {
            micesCarried++;
            playMenuScript.ChangeMiceCarriedNumber(micesCarried, maxCapability);
            fpsScript.MoveSpeed = fpsScript.MoveSpeed * 0.9f;
            miceCollected++;
        }
    }

    public void removeMice()
    {
        micesCarried = 0;
        playMenuScript.ChangeMiceCarriedNumber(micesCarried, maxCapability);
        playMenuScript.ChangeMiceLeftNumber(miceToCollect - miceCollected, miceToCollect);
        fpsScript.MoveSpeed = fpsScript.MoveSpeed * (float) Math.Pow(1.11f, micesCarried);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour
{
    //This class stores every potential hiding spot for cover.
    //It's used by the enemy AI to find unoccupied hiding spots

    public static CoverManager I;
    [HideInInspector] public CoveredSpot[] levelCover;

    void Awake()
    {
        CoverManager.I = this;
        levelCover = GetComponentsInChildren<CoveredSpot>();
    }
}

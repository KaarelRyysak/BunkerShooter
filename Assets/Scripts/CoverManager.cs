using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverManager : MonoBehaviour
{
    //This class stores every potential hiding spot for cover.
    //It's used by the enemy AI to find unoccupied hiding spots

    [HideInInspector] public CoveredSpot[] levelCover;

    void Awake()
    {
        levelCover = GetComponentsInChildren<CoveredSpot>();
    }
}

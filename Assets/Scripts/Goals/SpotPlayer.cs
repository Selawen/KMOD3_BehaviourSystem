using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayer : Goal
{
    // Start is called before the first frame update
    void Awake()
    {
        ChangePriority(0.5f);
        AddCondition("hasSpottedPlayer", true);
    }
}
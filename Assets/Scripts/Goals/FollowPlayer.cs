using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : Goal
{
    // Start is called before the first frame update
    void Awake()
    {
        ChangePriority(0.5f);
        AddCondition("nearPlayer", true);
        AddCondition("hidden", false);
    }
}

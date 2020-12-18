using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectPlayer : Goal
{
    // Start is called before the first frame update
    void Awake()
    {
        ChangePriority(1);
        AddCondition("playerInDanger", false);
    }
}

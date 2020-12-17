using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectPlayer : Goal
{
    // Start is called before the first frame update
    void Start()
    {
        priority = 0.5f;
        AddCondition("protectedPlayer", true);
    }

}

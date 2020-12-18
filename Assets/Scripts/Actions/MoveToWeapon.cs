﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToWeapon : Action
{

    public MoveToWeapon()
    {
        name = "moving towards weapon";
        addPrecondition("hasWeapon", false);
        addPrecondition("seesWeapon", true);
        addPrecondition("nearWeapon", false);
        addEffect("nearWeapon", true);
    }

    public override void OnEnterAction()
    {

    }

    public override void OnExitAction()
    {
        Reset();
    }

    public override void OnUpdateAction(Dictionary<string, object> worldState)
    {
        foreach (KeyValuePair<string, object> precondition in Preconditions)
        {
            bool match = false;
            foreach (KeyValuePair<string, object> s in worldState)
            {
                if (s.Equals(precondition))
                {
                    match = true;
                    break;
                }
            }
            if (!match)
            {
                actionFailed = true;
                return;
            }
        }
        if (!gameObject.GetComponent<NavMeshAgent>().hasPath)
        {
            actionCompleted = true;
        }
    }

    protected override void Reset()
    {
        actionCompleted = false;
        actionFailed = false;
    }
}

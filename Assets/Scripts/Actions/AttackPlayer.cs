using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackPlayer : Action
{
    private GameObject target;

    public AttackPlayer()
    {
        name = "attacking player";
        addPrecondition("hasWeapon", true); 
        addPrecondition("seesPlayer", true); 
        addPrecondition("nearPlayer", true); 
        addPrecondition("hasSpottedPlayer", true); 
        addPrecondition("playerKilled", false); 
        addEffect("playerKilled", true);
    }

    public override void OnEnterAction()
    {
        target = gameObject.GetComponent<IGoap>().GetTarget();
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
        if ((transform.position - target.transform.position).magnitude < 2)
        {
            actionCompleted = true;
            Destroy(target);
        }
        else if ((transform.position - target.transform.position).magnitude > 5)
        {
            actionFailed = true;
        }
    }

    protected override void Reset()
    {
        actionCompleted = false;
        actionFailed = false;
    }
}

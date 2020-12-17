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

    public override void OnUpdateAction()
    {
        
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

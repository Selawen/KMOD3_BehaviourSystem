using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayer : Action
{

    private GameObject target;
    private float elapsed;

    public MoveToPlayer()
    {
        name = "moving towards player";
        addPrecondition("hasWeapon", true);
        addPrecondition("seesPlayer", true);
        addPrecondition("nearPlayer", false);
        addPrecondition("playerKilled", false);
        addEffect("nearPlayer", true);
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
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path))
            {
                gameObject.GetComponent<NavMeshAgent>().SetPath(path);
            }
        }

        if ((transform.position-target.transform.position).magnitude < 1.5f)
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

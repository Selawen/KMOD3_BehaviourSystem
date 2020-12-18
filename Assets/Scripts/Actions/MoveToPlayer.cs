using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayer : Action
{

    private GameObject target;
    private float elapsed;
    private float nearDistance;
    private float lostDistance;

    public MoveToPlayer()
    {
        name = "moving towards player";
        AddPrecondition("hasWeapon", true);
        AddPrecondition("seesPlayer", true);
        AddPrecondition("nearPlayer", false);
        AddPrecondition("playerKilled", false);
        AddEffect("nearPlayer", true);
    }

    public override void OnEnterAction()
    {
        target = gameObject.GetComponent<IGoap>().GetTarget();
        nearDistance = gameObject.GetComponent<IGoap>().NearDistance();
        lostDistance = gameObject.GetComponent<IGoap>().LostDistance();
    }

    public override void OnExitAction()
    {
        gameObject.GetComponent<NavMeshAgent>().ResetPath();
        Reset();
    }

    public override void OnUpdateAction(Dictionary<string,object> worldState)
    {
        foreach(KeyValuePair<string, object> precondition in Preconditions)
        {
            bool match = false;
            foreach(KeyValuePair<string, object> s in worldState)
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

        if ((transform.position-target.transform.position).magnitude < nearDistance)
        {
            actionCompleted = true; 
        }
        else if ((transform.position - target.transform.position).magnitude > lostDistance)
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

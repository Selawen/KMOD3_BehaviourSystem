using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : Action
{
    [SerializeField] Vector3[] patrolPoints = new Vector3[4];
    private int patrolIndex = 0;

    [SerializeField] private GameObject target;

    private float visionAngle;
    private float visionDistance;

    public Patrol()
    {
        name = "patrolling";
        AddPrecondition("hasSpottedPlayer", false);
        AddPrecondition("playerKilled", false);
        AddEffect("hasSpottedPlayer", true);
    }

    public override void OnEnterAction()
    {
        visionAngle = gameObject.GetComponent<IGoap>().VisionAngle();
        visionDistance = gameObject.GetComponent<IGoap>().VisionDistance();
    }

    public override void OnExitAction()
    {
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

        Vector3 targetDir = target.transform.position - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.Angle(targetDir, forward);
        float distance = (target.transform.position - transform.position).magnitude;
        if (angle < visionAngle && distance < visionDistance && Physics.Raycast(transform.position, targetDir, out RaycastHit raycastHit))
        {
            if (raycastHit.transform.position == target.transform.position)
            {
                gameObject.GetComponent<IGoap>().SetTarget(target);
                actionCompleted = true;
                return;
            }
        }

        if (!gameObject.GetComponent<NavMeshAgent>().hasPath)
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, patrolPoints[patrolIndex], NavMesh.AllAreas, path))
            {
                gameObject.GetComponent<NavMeshAgent>().SetPath(path);
                if (patrolIndex == 3)
                {
                    patrolIndex = -1;
                }
                patrolIndex++;
            }
        }
    }

    protected override void Reset()
    {
        actionCompleted = false;
        actionFailed = false;
    }
}

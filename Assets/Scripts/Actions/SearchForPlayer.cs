using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchForPlayer : Action
{
    [SerializeField] private GameObject target;

    private float visionAngle;
    private float visionDistance;

    public SearchForPlayer()
    {
        name = "looking for player";
        addPrecondition("hasWeapon", true);
        addPrecondition("seesPlayer", false);
        addPrecondition("playerKilled", false);
        addEffect("seesPlayer", true);
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
            if (NavMesh.CalculatePath(transform.position, (transform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10))), NavMesh.AllAreas, path))
            {
                gameObject.GetComponent<NavMeshAgent>().SetPath(path);
            }
        }
    }

    protected override void Reset()
    {
        actionCompleted = false;
        actionFailed = false;
    }
}

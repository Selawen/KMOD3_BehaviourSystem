using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hide : Action
{
    [SerializeField] private GameObject target;

    public Hide()
    {
        name = "hiding";
        addPrecondition("hidden", false);
        addPrecondition("playerInDanger", true);
        addEffect("hidden", true);
    }

    public override void OnEnterAction()
    {
        NavMeshPath path = new NavMeshPath();
        Collider[] possibleHidingspots = Physics.OverlapSphere(transform.position, 20);

        Collider closest = null;
        float closestDist = 0;

        List<Collider> hidingspots = new List<Collider>();
        foreach (Collider possibleCover in possibleHidingspots)
        {
            if (possibleCover.gameObject.TryGetComponent<NavMeshObstacle>(out NavMeshObstacle obstacle))
            {
                hidingspots.Add(possibleCover);
            }
        }

        foreach (Collider cover in hidingspots)
        {
            if (closest == null)
            {
                // first one, so choose it for now
                closest = cover;
                closestDist = (cover.gameObject.transform.position - transform.position).magnitude;
            }
            else
            {
                // is this one closer than the last?
                float dist = (cover.gameObject.transform.position - transform.position).magnitude;
                if (dist < closestDist)
                {
                    // we found a closer one, use it
                    closest = cover;
                    closestDist = dist;
                }
            }
        }

        Vector3 hidingPos = (target.transform.position - closest.gameObject.transform.position) + (target.transform.position - closest.gameObject.transform.position).normalized;

        if (NavMesh.CalculatePath(transform.position, hidingPos, NavMesh.AllAreas, path))
        {
            gameObject.GetComponent<NavMeshAgent>().SetPath(path);
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SearchForWeapon : Action
{
    [SerializeField] private GameObject target;

    private float visionAngle;
    private float visionDistance;

    public SearchForWeapon()
    {
        name = "looking for weapon";
        addPrecondition("seesWeapon", false);
        addPrecondition("hasWeapon", false);
        addEffect("seesWeapon", true);
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

    public override void OnUpdateAction()
    {
        Vector3 targetDir = target.transform.position - transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.Angle(targetDir, forward);
        float distance = (target.transform.position - transform.position).magnitude;
        if (angle < visionAngle && distance < visionDistance)
        {
            gameObject.GetComponent<IGoap>().SetTarget(target);
            actionCompleted = true;
        }
        else if(!gameObject.GetComponent<NavMeshAgent>().hasPath)
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

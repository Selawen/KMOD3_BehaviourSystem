using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWeapon : Action
{
    private float startTime;
    private readonly float duration = 0.5f;

    public GetWeapon()
    {
        name = "picking up weapon";
        AddPrecondition("nearWeapon", true);
        AddPrecondition("seesWeapon", true);
        AddPrecondition("hasWeapon", false);
        AddEffect("hasWeapon", true);
    }

    public override void OnEnterAction()
    {
        startTime = Time.time;
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
        if (Time.time - startTime > duration)
        {
            gameObject.GetComponent<IGoap>().GetTarget().transform.parent = gameObject.transform;
            actionCompleted = true;
        }
    }

    protected override void Reset()
    {
        startTime = 0;
        actionCompleted = false;
        actionFailed = false;
    }
}

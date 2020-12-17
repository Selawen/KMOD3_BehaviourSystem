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
        addPrecondition("nearWeapon", true);
        addPrecondition("seesWeapon", true);
        addPrecondition("hasWeapon", false);
        addEffect("hasWeapon", true);
    }

    public override void OnEnterAction()
    {
        startTime = Time.time;
    }

    public override void OnExitAction()
    {
        Reset();
    }

    public override void OnUpdateAction()
    {
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

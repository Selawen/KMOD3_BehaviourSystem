using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : Action
{
    public Hide()
    {
        name = "hiding";
        addPrecondition("hidden", false);
        addEffect("hidden", true);
    }

    public override void OnEnterAction()
    {
        throw new System.NotImplementedException();
    }

    public override void OnExitAction()
    {
        throw new System.NotImplementedException();
    }

    public override void OnUpdateAction()
    {
        throw new System.NotImplementedException();
    }

    protected override void Reset()
    {
        throw new System.NotImplementedException();
    }
}

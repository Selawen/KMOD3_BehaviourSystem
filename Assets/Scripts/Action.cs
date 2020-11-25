using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public int cost;

    public abstract bool IsViable();

    public abstract void OnEnterAction();
    public abstract void OnUpdateAction();
    public abstract void OnExitAction();

    public abstract void OnActionCompleted();
    public abstract void OnActionFailed();
}

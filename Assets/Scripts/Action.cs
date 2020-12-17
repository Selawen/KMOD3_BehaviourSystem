using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    private Dictionary<string, object> preconditions;
    private Dictionary<string, object> effects;

    protected int cost;
    protected bool actionCompleted = false;
    protected bool actionFailed = false;

    public string name;

    public Action()
    {
        preconditions = new Dictionary<string, object>();
        effects = new Dictionary<string, object>();
    }

    public void addPrecondition(string key, object value)
    {
        preconditions.Add(key, value);
    }

    public void removePrecondition(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in preconditions)
        {
            if (kvp.Key.Equals(key))
                remove = kvp;
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
            preconditions.Remove(remove.Key);
    }

    public void addEffect(string key, object value)
    {
        effects.Add(key, value);
    }

    public void removeEffect(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in effects)
        {
            if (kvp.Key.Equals(key))
                remove = kvp;
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
            effects.Remove(remove.Key);
    }

    public Dictionary<string, object> Preconditions
    {
        get
        {
            return preconditions;
        }
    }

    public Dictionary<string, object> Effects
    {
        get
        {
            return effects;
        }
    }

    public int Cost
    {
        get
        {
            return cost;
        }
    }

    public bool ActionCompleted
    {
        get
        {
            return actionCompleted;
        }
    }

    public bool ActionFailed
    {
        get
        {
            return actionFailed;
        }
    }

    protected abstract void Reset();

    public abstract void OnEnterAction();
    public abstract void OnUpdateAction();
    public abstract void OnExitAction();
}

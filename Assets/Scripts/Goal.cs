using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Goal : MonoBehaviour
{
    public float priority;
    
    private Dictionary<string, object> goalState = new Dictionary<string, object>();

    public Goal()
    {
        priority = 0;
        goalState = new Dictionary<string, object>();
    }

    public Dictionary<string, object> GoalState
    {
        get
        {
            return goalState;
        }
    }

    public void AddCondition(string key, object value)
    {
        goalState.Add(key, value);
    }

    public void RemoveCondition(string key)
    {
        KeyValuePair<string, object> remove = default(KeyValuePair<string, object>);
        foreach (KeyValuePair<string, object> kvp in goalState)
        {
            if (kvp.Key.Equals(key))
                remove = kvp;
        }
        if (!default(KeyValuePair<string, object>).Equals(remove))
            goalState.Remove(remove.Key);
    }
}

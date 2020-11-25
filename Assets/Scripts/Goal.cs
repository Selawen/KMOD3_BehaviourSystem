using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Goal : ScriptableObject
{
    public int priority;

    internal Stack<Action> actionStack = new Stack<Action>();
}

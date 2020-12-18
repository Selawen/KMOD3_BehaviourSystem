using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActionTemplate : Action
{

    public ActionTemplate()
    {
        name = "action name";
        AddPrecondition("precondition", true);
        AddEffect("effect", true);
    }

    public override void OnEnterAction()
    {

    }

    public override void OnExitAction()
    {
        Reset();
    }

    public override void OnUpdateAction(Dictionary<string,object> worldState)
    {
        foreach(KeyValuePair<string, object> precondition in Preconditions)
        {
            bool match = false;
            foreach(KeyValuePair<string, object> s in worldState)
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
    }

    protected override void Reset()
    {
        actionCompleted = false;
        actionFailed = false;
    }
}

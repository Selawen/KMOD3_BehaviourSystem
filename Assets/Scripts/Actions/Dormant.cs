using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Dormant : Action
{
    private IGoap parent;

    public Dormant(IGoap dormantEntity)
    {
        name = "dormant";
        parent = dormantEntity;
    }

    public override void OnEnterAction()
    {
        foreach(KeyValuePair<string, object> worldCondition in parent.GetWorldState())
        {
           
            addPrecondition(worldCondition.Key, worldCondition.Value);
        }
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
        Destroy(this);
        //actionCompleted = false;
        //actionFailed = false;
        //foreach (KeyValuePair<string, object> precondition in Preconditions)
        //{
        //    removePrecondition(precondition.Key);
        //}
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ally : MonoBehaviour, IGoap
{
    private Dictionary<string, object> worldState;
    [SerializeField] private Queue<Action> actionPlan;

    private NavMeshAgent meshAgent;
    private NavMeshPath path;
    [SerializeReference] private GameObject target;


    private float visionAngle = 70.0f;
    private float visionDistance = 30.0f;

    void Start()
    {
        worldState = new Dictionary<string, object>();
        actionPlan = new Queue<Action>();
        AddStartState();
        meshAgent = gameObject.GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
    }

    private void AddStartState()
    {
        //ToDo: make this non-static
        worldState.Add("seesPlayer", false);
        worldState.Add("nearPlayer", false);
    }

    public void ActionFinished(Action finishedAction)
    {
        /**
 * Apply the stateChange to the currentState
 */

        Dictionary<string, object> state = new Dictionary<string, object>();
        // copy the KVPs over as new objects
        foreach (KeyValuePair<string, object> s in worldState)
        {
            state.Add(s.Key, s.Value);
        }

        foreach (KeyValuePair<string, object> change in finishedAction.Effects)
        {
            // if the key exists in the current state, update the Value
            bool exists = false;

            foreach (KeyValuePair<string, object> s in state)
            {
                if (s.Key.Equals(change.Key))
                {
                    exists = true;
                    state.Remove(s.Key);
                    state.Add(change.Key, change.Value);
                    break;
                }
            }

            if (exists)
            {
                //state.RemoveWhere((KeyValuePair<string, object> kvp) => { return kvp.Key.Equals(change.Key); });
                //KeyValuePair<string, object> updated = new KeyValuePair<string, object>(change.Key, change.Value);
                //state.Add(updated);
            }
            // if it does not exist in the current state, add it
            else
            {
                state.Add(change.Key, change.Value);
            }
        }
        worldState = state;
    }
    /*
    public Dictionary<string, object> SetNewGoal()
    {
        throw new System.NotImplementedException();
    }
    */
    public Dictionary<string, object> GetWorldState()
    {
        return worldState;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    public void SetTarget(GameObject _target)
    {
        target = _target;
        if (NavMesh.CalculatePath(gameObject.transform.position, target.transform.position, NavMesh.AllAreas, path))
        {
            meshAgent.SetPath(path);
        }
    }

    public bool moveAgent(Action nextAction)
    {
        throw new System.NotImplementedException();
    }

    public void planAborted(Action aborter)
    {
        throw new System.NotImplementedException();
    }

    public void planFailed(Dictionary<string, object> failedGoal)
    {
        throw new System.NotImplementedException();
    }

    public void planFound(Queue<Action> actions)
    {
        actionPlan = actions;
    }

    public float VisionAngle()
    {
        return visionAngle;
    }

    public float VisionDistance()
    {
        return visionDistance;
    }
}


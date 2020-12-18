using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IGoap
{
    private Dictionary<string, object> worldState;
    [SerializeField] private Queue<Action> actionPlan;

    private NavMeshAgent meshAgent;
    private NavMeshPath path;
    [SerializeReference] private GameObject target;
    [SerializeReference] private GameObject player;

    [SerializeReference] private float visionAngle = 50.0f;
    [SerializeReference] private float visionDistance = 30.0f;
    [SerializeReference] private float nearDistance = 1.5f;
    [SerializeReference] private float lostDistance = 10.0f;

    void Awake()
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
        worldState.Add("seesWeapon", false);
        worldState.Add("hasWeapon", false);
        worldState.Add("seesPlayer", false);
        worldState.Add("playerKilled", false);
        worldState.Add("nearWeapon", false);
        worldState.Add("nearPlayer", false);
        worldState.Add("hasSpottedPlayer", false);
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

    public bool CheckForPrecondition(string _precondition, object _value)
    {
        foreach(KeyValuePair<string, object> kvp in worldState)
        {
            if (kvp.Key.Equals(_precondition) && kvp.Value.Equals(_value))
            {
                return true;
            }
        }
        return false;
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

    public float NearDistance()
    {
        return nearDistance;
    }

    public float LostDistance()
    {
        return lostDistance;
    }

    public void UpdateWorldState()
    {
        if (player == null)
        {
            worldState["playerKilled"] = true;
            return;
        }
        if ((transform.position - player.transform.position).magnitude > nearDistance)
        {
            worldState["nearPlayer"] = false;
        }
        if ((transform.position - player.transform.position).magnitude > lostDistance)
        {
            worldState["seesPlayer"] = false;
        }
        if (Physics.Raycast(transform.position, (player.transform.position - transform.position), out RaycastHit raycastHit))
        {
            if (raycastHit.transform.position != player.transform.position)
            {
                worldState["seesPlayer"] = false;
                worldState["hasSpottedPlayer"] = false;
            }
        }
    }
}

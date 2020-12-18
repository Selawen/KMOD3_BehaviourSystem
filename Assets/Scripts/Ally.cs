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
    [SerializeReference] private GameObject player;
    private Enemy[] enemies;

    [SerializeReference] private float visionAngle = 70.0f;
    [SerializeReference] private float visionDistance = 30.0f;
    [SerializeReference] private float nearDistance = 5.0f;
    [SerializeReference] private float lostDistance = 100.0f;

    void Awake()
    {
        worldState = new Dictionary<string, object>();
        actionPlan = new Queue<Action>();
        AddStartState();
        meshAgent = gameObject.GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        enemies = FindObjectsOfType<Enemy>();
    }

    private void AddStartState()
    {
        //ToDo: make this non-static
        worldState.Add("seesPlayer", false);
        worldState.Add("nearPlayer", false);
        worldState.Add("hasWeapon", true);
        worldState.Add("hidden", false);
        worldState.Add("smokebombThrown", false);
        worldState.Add("playerKilled", false);
        worldState.Add("playerInDanger", false);
    }

    public void ActionFinished(Action finishedAction)
    {
        /**
 * Apply the stateChange to the currentState
 */
        //actionPlan.Dequeue();

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
                    state.Remove(change.Key);
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

        foreach(Enemy enemy in enemies)
        {
            if (enemy.CheckForPrecondition("seesPlayer", true))
            {
                worldState["playerInDanger"] = true;
                target = enemy.gameObject;
                return;
            }
        }
        worldState["playerInDanger"] = false;
        target = player;

        if ((transform.position - player.transform.position).magnitude > nearDistance)
        {
            worldState["nearPlayer"] = false;
        }
        if ((transform.position - player.transform.position).magnitude > lostDistance)
        {
            worldState["seesPlayer"] = false;
        }
    }
}


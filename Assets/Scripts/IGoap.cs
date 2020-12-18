using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Any agent that wants to use GOAP must implement
 * this interface. It provides information to the GOAP
 * planner so it can plan what actions to use.
 * 
 * It also provides an interface for the planner to give 
 * feedback to the Agent and report success/failure.
 */
public interface IGoap
{
    float VisionAngle();
    float VisionDistance();
    float NearDistance();
    float LostDistance();

    /**
	 * The starting state of the Agent and the world.
	 * Supply what states are needed for actions to run.
	 */
    Dictionary<string, object> GetWorldState();


    void UpdateWorldState();


    /**
	 * A plan was found for the supplied goal.
	 * These are the actions the Agent will perform, in order.
	 */
    void planFound(Queue<Action> actions);

    /**
	 * All actions are complete and the goal was reached. Hooray!
	 */
    void ActionFinished(Action finishedAction);

    /**
	 * Get the object the agent is currently targeting
	 */
    GameObject GetTarget();

    /**
	 * set a new object for the agent to target
	 */
    void SetTarget(GameObject _target);

}

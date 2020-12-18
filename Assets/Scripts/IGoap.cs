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
	 * Give the planner a new goal so it can figure out 
	 * the actions needed to fulfill it.
	 */
    //not necessary 
    //Dictionary<string, object> SetNewGoal();

    /**
	 * No sequence of actions could be found for the supplied goal.
	 * You will need to try another goal
	 */
    void planFailed(Dictionary<string, object> failedGoal);

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

    /**
	 * One of the actions caused the plan to abort.
	 * That action is returned.
	 */
    void planAborted(Action aborter);

    /**
	 * Called during Update. Move the agent towards the target in order
	 * for the next action to be able to perform.
	 * Return true if the Agent is at the target and the next action can perform.
	 * False if it is not there yet.
	 */
    bool moveAgent(Action nextAction);
}

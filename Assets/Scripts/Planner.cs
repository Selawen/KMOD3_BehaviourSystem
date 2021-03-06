﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner
{
    public Queue<Action> PlanActions(GameObject agent,
                                  HashSet<Action> availableActions,
                                  HashSet<Goal> availableGoals,
                                  Dictionary<string, object> worldState)
    {
        HashSet<Goal> usableGoals = new HashSet<Goal>();

        foreach (Goal g in availableGoals)
        {
            usableGoals.Add(g);
        }

        Goal thisGoal = SetGoal(usableGoals);

        // check what actions can run
        HashSet<Action> usableActions = new HashSet<Action>();
        foreach (Action a in availableActions)
        {
            foreach (string precondition in a.Preconditions.Keys)
            {
                if (worldState.ContainsKey(precondition))
                {
                    usableActions.Add(a);
                }
            }
        }

        

        // we now have all actions that can run, stored in usableActions

        // build up the tree and record the leaf nodes that provide a solution to the goal.
        List<Node> leaves = new List<Node>();

        // build graph
        Node start = new Node(null, 0, worldState, null);


        //bool success = BuildGraph(start, leaves, availableActions, thisGoal.GoalState);

        //if (!success)
        while (!BuildGraph(start, leaves, availableActions, thisGoal.GoalState))
        {
            // oh no, we didn't get a plan, try with another goal
            usableGoals.Remove(thisGoal);
            if (usableGoals.Count > 0)
            {
                thisGoal = SetGoal(usableGoals);
            }
            else
            {
                Queue<Action> dormantQueue = new Queue<Action>();

                //Debug.Log(thisGoal.name+ ": " + a.name);
                dormantQueue.Enqueue(new Dormant(agent.GetComponent<IGoap>()));
                return dormantQueue;
            }
        }

        // get the cheapest leaf
        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
                cheapest = leaf;
            else
            {
                if (leaf.runningCost < cheapest.runningCost)

                    cheapest = leaf;
            }
        }

        // get its node and work back through the parents
        List<Action> result = new List<Action>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action); // insert the action in the front
            }
            n = n.parent;
        }
        // we now have this action list in correct order

        Queue<Action> queue = new Queue<Action>();
        foreach (Action a in result)
        {
            //Debug.Log(thisGoal.name+ ": " + a.name);
            queue.Enqueue(a);
        }
        
        /*
        //ToDo: check if this implementation works
        Queue<Action> queue = new Queue<Action>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                queue.Enqueue(n.action); // insert the action in the front
            }
            n = n.parent;
        }
        // we now have this action list in correct order
        */

        // hooray we have a plan!
        return queue;
    }

    private Goal SetGoal(HashSet<Goal> availableGoals)
    {
        Goal tempGoal = default;

        foreach (Goal g in availableGoals)
        {
            if (tempGoal == null)
            {
                tempGoal = g;
            }
            else if (g.Priority > tempGoal.Priority)
            {
                tempGoal = g;
            }
        }
        return tempGoal;
    }





    /**
	 * Returns true if at least one solution was found.
	 * The possible paths are stored in the leaves list. Each leaf has a
	 * 'runningCost' value where the lowest cost will be the best action
	 * sequence.
	 */
    private bool BuildGraph(Node parent, List<Node> leaves, HashSet<Action> usableActions, Dictionary<string, object> goal)
    {
        bool foundOne = false;

        // go through each action available at this node and see if we can use it here
        foreach (Action action in usableActions)
        {

            // if the parent state has the conditions for this action's preconditions, we can use it here
            if (InState(action.Preconditions, parent.state))
            {

                // apply the action's effects to the parent state
                Dictionary<string, object> currentState = PopulateState(parent.state, action.Effects);
                //Debug.Log(GoapAgent.prettyPrint(currentState));
                Node node = new Node(parent, parent.runningCost + action.Cost, currentState, action);

                if (InState(goal, currentState))
                {
                    // we found a solution!
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    // not at a solution yet, so test all the remaining actions and branch out the tree
                    HashSet<Action> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                        foundOne = true;
                }
            }
        }

        return foundOne;
    }

    /**
     * Create a subset of the actions excluding the removeMe one. Creates a new set.
     */
    private HashSet<Action> ActionSubset(HashSet<Action> actions, Action removeMe)
    {
        HashSet<Action> subset = new HashSet<Action>();
        foreach (Action a in actions)
        {
            if (!a.Equals(removeMe))
                subset.Add(a);
        }
        return subset;
    }

    /**
     * Check that all items in 'test' are in 'state'. If just one does not match or is not there
     * then this returns false.
     */
    private bool InState(Dictionary<string, object> test, Dictionary<string, object> state)
    {
        bool allMatch = true;
        foreach (KeyValuePair<string, object> t in test)
        {
            bool match = false;
            foreach (KeyValuePair<string, object> s in state)
            {
                if (s.Equals(t))
                {
                    match = true;
                    break;
                }
            }
            if (!match)
            {
                allMatch = false;
                break;
            }
        }
        return allMatch;
    }

    /**
     * Apply the stateChange to the currentState
     */
    private Dictionary<string, object> PopulateState(Dictionary<string, object> currentState, Dictionary<string, object> stateChange)
    {
        Dictionary<string, object> state = new Dictionary<string, object>();
        // copy the KVPs over as new objects
        foreach (KeyValuePair<string, object> s in currentState)
        {
            state.Add(s.Key, s.Value);
        }

        foreach (KeyValuePair<string, object> change in stateChange)
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
        return state;
    }

    /**
     * Used for building up the graph and holding the running costs of actions.
     */
    private class Node
    {
        public Node parent;
        public float runningCost;
        public Dictionary<string, object> state;
        public Action action;

        public Node(Node parent, float runningCost, Dictionary<string, object> state, Action action)
        {
            this.parent = parent;
            this.runningCost = runningCost;
            this.state = state;
            this.action = action;
        }
    }
}
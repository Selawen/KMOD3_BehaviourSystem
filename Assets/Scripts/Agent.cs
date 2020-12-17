using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Agent : MonoBehaviour
{
    private HashSet<Action> availableActions;
    private HashSet<Goal> availableGoals;

    private Queue<Action> currentActions;
    [SerializeField] private Action action;

    private IGoap dataProvider; // this is the implementing class that provides our world data and listens to feedback on planning
    private Planner planner;

    [SerializeField] private TextMeshProUGUI UI;

    // Start is called before the first frame update
    void Start()
    {
        availableActions = new HashSet<Action>();
        availableGoals = new HashSet<Goal>();
        currentActions = new Queue<Action>();
        planner = new Planner();

        dataProvider = gameObject.GetComponent<IGoap>();

        loadActions();
        loadGoals();
        newPlan();
    }

    // Update is called once per frame
    void Update()
    {
        // Create a new plan if the last one finished
        if (!hasActionPlan())
        {
            newPlan();
        }

        action = currentActions.Peek();

        if (action.ActionCompleted)
        {
            action.OnExitAction();
            dataProvider.ActionFinished(action);
            currentActions.Dequeue();
            action = currentActions.Peek();
            action.OnEnterAction();
        }
        else if (action.ActionFailed)
        {
            action.OnExitAction();
            currentActions.Clear();
            return;
        }

        action.OnUpdateAction();
        UI.text = action.name;
    }

    public void addAction(Action a)
    {
        availableActions.Add(a);
    }

    public Action getAction(Action action)
    {
        foreach (Action g in availableActions)
        {
            if (g.GetType().Equals(action))
                return g;
        }
        return null;
    }

    public void removeAction(Action action)
    {
        availableActions.Remove(action);
    }

    private bool hasActionPlan()
    {
        return currentActions.Count > 0;
    }

    private void loadActions()
    {
        Action[] actions = gameObject.GetComponents<Action>();
        foreach (Action a in actions)
        {
            availableActions.Add(a);
        }
    }

    private void loadGoals()
    {
        Goal[] goals = gameObject.GetComponents<Goal>();
        foreach (Goal g in goals)
        {
            availableGoals.Add(g);
        }
    }

    private void newPlan()
    {
        Dictionary<string, object> worldState = dataProvider.GetWorldState();

        currentActions = planner.PlanActions(gameObject, availableActions, availableGoals, worldState);

        dataProvider.planFound(currentActions);
        action = currentActions.Peek();
        action.OnEnterAction();
    }
}

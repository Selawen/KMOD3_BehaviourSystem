using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ThrowSmokebomb : Action
{
    [SerializeReference] private GameObject enemy;
    [SerializeField] private GameObject player;

    private float startTime;
    [SerializeField] private float duration = 0.3f;

    public ThrowSmokebomb()
    {
        name = "throwing smoke bomb";
        addPrecondition("hidden", true);
        addPrecondition("playerInDanger", true);
        addPrecondition("smokebombThrown", false);
        addEffect("smokebombThrown", true);
        addEffect("playerInDanger", false);
    }

    public override void OnEnterAction()
    {
        startTime = Time.time;
        enemy = gameObject.GetComponent<IGoap>().GetTarget();
    }

    public override void OnExitAction()
    {
        Reset();
    }

    public override void OnUpdateAction(Dictionary<string, object> worldState)
    {
        foreach (KeyValuePair<string, object> precondition in Preconditions)
        {
            bool match = false;
            foreach (KeyValuePair<string, object> s in worldState)
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


        if (Time.time - startTime > duration)
        {
            GameObject smokebomb = ObjectFactory.CreatePrimitive(PrimitiveType.Sphere);
            smokebomb.transform.localScale = new Vector3(5, 5, 5);
            Vector3 smokePos = Vector3.Lerp(player.transform.position, enemy.transform.position, 0.3f);
            smokebomb.transform.position = smokePos;
            smokebomb.GetComponent<Collider>().isTrigger = true;
            Destroy(smokebomb, 5);

            actionCompleted = true;
        }
    }

    protected override void Reset()
    {
        actionCompleted = false;
        actionFailed = false;
    }
}

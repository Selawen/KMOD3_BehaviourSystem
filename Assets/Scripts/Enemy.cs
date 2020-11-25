using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private List<Action> availableActions;
    private List<Goal> availableGoals;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum Effect
    {
        SeePlayer = (1 << 0),
        HasWeapon = (1 << 1),
        SeeWeapon = (1 << 2),
        NearWeapon = (1 << 3),
        NearPlayer = (1 << 4),
    }
}

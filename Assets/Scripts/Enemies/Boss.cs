using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField]
    [Range(1.0f, 10000.0f)]
    protected float health;

    [SerializeField]
    [Range(1.0f, 100.0f)]
    protected float speed;

    protected BossStateMachine stateMachine;

    public Boss()
    {
        stateMachine = new BossStateMachine();
    }

    public string GetCurrentState()
    {
        if(stateMachine == null || stateMachine.GetState() == null)
        {
            return "None";
        }

        return stateMachine.GetState().stateName;
    }
}

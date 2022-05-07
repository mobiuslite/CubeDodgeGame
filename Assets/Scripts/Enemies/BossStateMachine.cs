using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine
{
    GameObject projectile;

    List<BossState> states;
    BossState curState;

    bool isRunning;

    public BossStateMachine()
    {
        states = new List<BossState>();
        isRunning = false;
    }

    public void AddState(BossState state)
    {
        states.Add(state);
        state.SetParent(this);
    }

    public void Update(float dt)
    {
        if (this.isRunning)
        {
            if (curState == null)
            {
                isRunning = false;
                return;
            }

            curState.Update(dt);

            if (curState.IsDone())
            {
                BossState newState = curState.GetTransition();
                TransitionToState(newState);
            }
        }
    }

    public void SetProjectile(GameObject prj)
    {
        projectile = prj;
    }

    public GameObject GetProjectile()
    {
        return projectile;
    }

    public BossState GetState()
    {
        return curState;
    }

    public void Start()
    {
        if (states.Count > 0)
        {
            isRunning = true;
        }

        TransitionToState(states[0]);
    }
    public void Reset()
    {
        if (curState != null)
            curState.OnExit();

        curState = null;
        this.isRunning = false;
    }

    void TransitionToState(BossState state)
    {
        if (curState != null)
            curState.OnExit();

        curState = state;

        if (curState != null)
            curState.OnEnter();
    }
}

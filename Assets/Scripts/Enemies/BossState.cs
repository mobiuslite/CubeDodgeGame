using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState
{
    public string stateName;

    Dictionary<int, BossState> transitions;
    protected BossStateMachine parent;

    protected int curCondition;

    private BossState()
    {
        curCondition = 0;
    }

    public BossState(string name)
    {
        transitions = new Dictionary<int, BossState>();
        curCondition = 0;
        stateName = name;
    }

    public virtual void Update(float dt)
    {
    }
    public virtual void OnEnter()
    {
    }
    public virtual void OnExit()
    {
    }

    public bool IsDone()
    {
        return curCondition != 0;
    }

    public void AddTransition(int condition, BossState state)
    {
        transitions.Add(condition, state);
    }

    public void SetParent(BossStateMachine p)
    {
        parent = p;
    }

    public BossState GetTransition()
    {
        if (!IsDone())
            return null;

        return transitions[curCondition];
    }
}

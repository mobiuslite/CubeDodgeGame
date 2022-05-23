using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A state machine that controls how the boss attacks.
/// Make sure to call update every frame
/// </summary>
public class BossStateMachine
{
    Dictionary<string, GameObject> damageObjects;

    List<BossState> states;
    BossState curState;

    bool isRunning;

    public BossStateMachine()
    {
        states = new();
        damageObjects = new();

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

    public void AddDamageObject(string key, GameObject value)
    {
        damageObjects.Add(key, value);
    }

    public bool TryGetDamageObject(string key, out GameObject obj)
    {
        bool found = damageObjects.TryGetValue(key, out obj);
        if (!found)
        {
            Debug.LogError($"Key \"{key}\" not found in damage object dictionary");
        }

        return found; 
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

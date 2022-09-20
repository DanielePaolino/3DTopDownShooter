using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, State> states;

    public State CurrentState { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (CurrentState == null)
            CurrentState = states.Values.First();

        Type nextState = CurrentState.Tick();
        if(nextState != null && nextState != CurrentState?.GetType() && states.ContainsKey(nextState))
        {
            SwitchToNextState(nextState);
        }
    }

    public void SetStates(Dictionary<Type, State> states)
    {
        this.states = states;
    }

    private void SwitchToNextState(Type stateType)
    {
        CurrentState = states[stateType];
    }
}

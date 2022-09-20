using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected GameObject enemy;

    public State(GameObject go)
    {
        enemy = go;
    }
    public abstract Type Tick();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object
{
    public Map map;
    public GameObject gameObject;
    virtual public string objectName { get; }
    abstract public StateEvent stateCheck(float time);
    abstract public void execute(Command command);
    abstract public void startObject();
    abstract public void updateObject(float time);
}


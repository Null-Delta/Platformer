using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : Object
{

    float nowTime = 0f;
    float needTime = 0f;

    bool isActive = true;

    public override void execute(Command command)
    {
        switch (command.name) {
            case "Restart":
                isActive = true;
                nowTime = 0f;
            break; 
        }
    }

    public override void resetObject()
    {
        nowTime = 0f;
        isActive = true;
    }

    public override void startObject()
    {
        nowTime = 0f;
    }

    public override void updateObject(float time)
    {
        Debug.Log("now!");
        if(isActive) {
            nowTime += time;
            if(nowTime >= needTime) {
                nowTime = 0f;
                isActive = false;
                map.executeGroup(events["onTimer"]);
            }
        }
    }

    public Timer(float t, List<Command> commands, bool active) {
        needTime = t;
        events = new Dictionary<string, List<Command>>();
        events["onTimer"] = commands;
        isActive = active;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Counter: Object {
    int needCount = 0;
    int nowCount = 0;

    public Counter(int count, List<Command> onCount, List<Command> onNotCount) {
        needCount = count;

        events = new Dictionary<string, List<Command>>();
        events["onCount"] = onCount;
        events["onNotCount"] = onNotCount;
    }

    public override void execute(Command command)
    {
        switch(command.name) {
            case "Add":
                nowCount++;
                if(nowCount == needCount) {
                    map.executeGroup(events["onCount"]);
                }
            break;
            case "Delete":
                if(nowCount == needCount) {
                    map.executeGroup(events["onNotCount"]);
                }
                nowCount--;
            break;
        }
    }

    public override void resetObject()
    {
        nowCount = 0;
    }

    public override void startObject()
    {
        
    }

    public override void updateObject()
    {
        
    }
}
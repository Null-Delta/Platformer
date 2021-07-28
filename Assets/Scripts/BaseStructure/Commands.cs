using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Loop {
    public static void Around(Vector2Int point, Action<Vector2Int> action) {
        action(point + new Vector2Int(-1,0));
        action(point + new Vector2Int(-1,-1));
        action(point + new Vector2Int(0,-1));
        action(point + new Vector2Int(1,-1));
        action(point + new Vector2Int(1,0));
        action(point + new Vector2Int(1,1));
        action(point + new Vector2Int(0,1));
        action(point + new Vector2Int(-1,1));
    }
}

public class Command {
    public string name;
    public int groupID;
    public List<string> inputs;
    public List<string> types;

    public Command(string n, int id, List<string> inp, List<string> info) {
        name = n;
        groupID = id;
        inputs = inp;
        types = info;
    }
}
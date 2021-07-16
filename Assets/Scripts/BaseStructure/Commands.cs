using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Command {
    public string name;
    public List<string> inputs;
    public List<string> types;
}

public class Action {
    public int groupID;
    public List<Command> commands;
}
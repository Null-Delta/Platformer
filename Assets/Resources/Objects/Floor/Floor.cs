using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ConnectedObject
{

    public override string objectName => "Floor";

    public override bool isTile => true;

    public Floor(int x, int y): base(x,y) { 
        order = ObjectOrder.floor;
        isCollisiable = false;
        position = new Vector2(x,y);
    }

    public Floor(): base(0,0) {
        order = ObjectOrder.floor;
        isCollisiable = false;
        position = new Vector2(0,0);
    }
}

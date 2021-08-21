using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ConnectedObject
{

    public override string objectName => "Wall";
    public override bool isTile => true;
    public override void startObject()
    {
        base.startObject();

    }

    public Wall(int x, int y): base(x,y) {  
        order = ObjectOrder.wall;
        isCollisiable = true;
    }

    public Wall(): base(0,0) {
        order = ObjectOrder.wall;
        isCollisiable = true;
    }    
}

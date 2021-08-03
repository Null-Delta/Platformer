using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ConnectedObject
{

    public override string objectName => "Wall";

    public override void startObject()
    {
        base.startObject();
        isCollisiable = true;

        setupStyle((int)position.x, (int)position.y);
        order = ObjectOrder.wall;
        setupOrder();
    }

    public Wall(int x, int y): base(x,y) {    }

    public Wall(): base(0,0) {}    
}

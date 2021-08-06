using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ConnectedObject
{

    public override string objectName => "Floor";

    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        gameObject.transform.position = position;
        order = ObjectOrder.floor;

        setupStyle((int)position.x, (int)position.y);
        setupOrder();
    }

    public Floor(int x, int y): base(x,y) { }

    public Floor(): base(0,0) {}
}

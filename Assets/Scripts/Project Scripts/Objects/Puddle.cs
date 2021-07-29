using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : ConnectedObject
{

    public override string objectName => "Puddle";

    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        gameObject.transform.position = position;
        
        setupStyle((int)position.x, (int)position.y);
        order = ObjectOrder.onFloor;
    }

    public Puddle(int x, int y) {
        position = new Vector2(x,y);
    }

    public Puddle() {}
    

    
}

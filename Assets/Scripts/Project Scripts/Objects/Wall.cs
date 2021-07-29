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
        gameObject.transform.position = position;

        setupStyle((int)position.x, (int)position.y);
        order = ObjectOrder.wall;
    }

    public Wall(int x, int y) {
        position = new Vector2(x,y);
    }

    public Wall() {}
    

    
}

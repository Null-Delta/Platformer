using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : ConnectedObject
{

    public override string objectName => "Wall";
    

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-2);
        isCollisiable = true;
        setupStyle((int)position.x, (int)position.y);
    }

    public Wall(int x, int y) {
        position = new Vector2(x,y);
    }

    public Wall() {}
    

    
}

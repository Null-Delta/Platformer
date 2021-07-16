using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : StaticMapObject
{

    public override string objectName => "Wall";

    public override void startObject()
    {
        base.startObject();
        isDecoration = false;
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y);
    }

    public Wall(int x, int y) {
        position = new Vector2(x,y);
    }
}

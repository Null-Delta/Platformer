using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : StaticMapObject
{

    public override string objectName => "Floor";

    public override void startObject()
    {
        base.startObject();
        isDecoration = true;
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)position.y - 1;
    }

    public Floor(int x, int y) {
        position = new Vector2(x,y);
    }

    public Floor() {
        position = new Vector2(0,0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : ConnectedObject
{

    public override string objectName => "Floor";

    public override void startObject()
    {
        base.startObject();
        isDecoration = true;
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)position.y - 1;

        setupStyle((int)position.x, (int)position.y);
    }

    public Floor(int x, int y) {
        position = new Vector2(x,y);
    }

    public Floor() {}
}

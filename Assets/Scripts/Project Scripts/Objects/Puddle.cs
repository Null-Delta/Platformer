using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : ConnectedObject
{

    public override string objectName => "Puddle";

    public override void startObject()
    {
        base.startObject();
        isCollisiable = true;
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y-1);

        
        setupStyle((int)position.x, (int)position.y);
    }

    public Puddle(int x, int y) {
        position = new Vector2(x,y);
    }

    public Puddle() {}
    

    
}

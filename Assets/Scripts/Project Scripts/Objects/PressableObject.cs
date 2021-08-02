using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressableObject : MapObject
{
    public override string objectName => "OnPressObject";

    public virtual void OnPressEnd(WalkableObject walker)
    {
        
    }
    public virtual void OnPressStart(WalkableObject walker)
    {
        
    }

    public override void startObject()
    {
        base.startObject();
        isCollisiable = false;
        gameObject.transform.position = position;
        order = ObjectOrder.onFloor;
    }

    public PressableObject(int x, int y): base(x,y) { }
    

}

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
        order = ObjectOrder.onFloor;
        var tmpList = map.getMapObjects<WalkableObject>((int)position.x, (int)position.y, x => x is WalkableObject);
        if (tmpList != null)
            OnPressStart(tmpList[0]);
    }

    public PressableObject(int x, int y): base(x,y) { }
    

}

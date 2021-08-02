using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : PushableObject
{
    int nowDirection;
    public override string objectName => "Box";

    public override void onEndWalk()
    {
        nowDirection = -1;
    }

    public override bool canMoveOn(Vector2Int point)
    {
        return map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable == true) == null;
    }

    public Box(int x, int y): base(x,y) {
        
    }

    public override void startObject()
    {
        base.startObject();
        nowDirection = -1;
        stayDelay = 0f;
        isCollisiable = true;
        order = ObjectOrder.wall;
    }
}

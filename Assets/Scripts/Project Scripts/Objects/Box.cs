using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : WalkableObject
{

    int nowDirection;

    public override string objectName => "Box";

    public override void onEndWalk()
    {
        nowDirection = -1;
    }
    public bool willMove() {
        return nowDirection != -1;
    }

    public void setDirection(int dir) {

        movement m = new movement();
        m.isAnimate = true;
        nowDirection = dir;

        switch (dir) {
            case 0:
                m.point = new Vector2Int(-1,0);
            break;
            case 1:
                m.point = new Vector2Int(0,1);
            break;
            case 2:
                m.point = new Vector2Int(1,0);
            break;
            case 3:
                m.point = new Vector2Int(0,-1);
            break;
        }

        if(map.getMapObjects<MapObject>((int)(mapLocation.x + m.point.x), (int)(mapLocation.y + m.point.y), x => x.isCollisiable == true) != null)
        {
            nowDirection = -1;
        } else {
            addMovement(m);
        }
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

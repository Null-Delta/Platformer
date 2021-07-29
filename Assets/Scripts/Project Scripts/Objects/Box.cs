using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Walker
{

    int nowDirection;

    public override string objectName => "Box";

    public override bool readyCheck()
    {
        return nowDirection != -1;
    }

    public override void onWalkFinish()
    {
        nowDirection = -1;
    }
    public bool willMove() {
        return nowDirection != -1;
    }

    public override void onWalkStart()
    {
        Vector2 tmpNewPosition = position+linearMove;
        if (isOnFloor)
            isOnFloor = false;
        var tmpList = map.getMapObjects<MovingFloor>((int)tmpNewPosition.x,(int)tmpNewPosition.y, x => x is MovingFloor);
        if(tmpList != null && tmpList[0].onMe ==null)                                   // обработка возможности вхождения на движущийся пол.
        {
            tmpList[0].addWalkerOn(tmpNewPosition ,this);
            isOnFloor = true;
        }
    }

    public void setDirection(int dir) {
        linearMove.x = 0;
        linearMove.y = 0;

        nowDirection = dir;

        switch (dir) {
            case 0:
                linearMove.x = -1;
            break;
            case 1:
                linearMove.y = 1;
            break;
            case 2:
                linearMove.x = 1;
            break;
            case 3:
                linearMove.y = -1;
            break;
        }

        if(map.getMapObjects<MapObject>((int)(position.x + linearMove.x), (int)(position.y + linearMove.y), x => x.isCollisiable == true) != null)
        {
            nowDirection = -1;
        }

        sum_time = 0f;

    }

    public Box(int x, int y): base(x,y) {
        
    }

    public override void startObject()
    {
        base.startObject();
        nowDirection = -1;
        move_delay = 0f;
        animation_time = 0.15f;
        isCollisiable = true;
    }
}

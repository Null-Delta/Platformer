using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsualStalker : Seeker
{
    public override string objectName => "UsualStalker";

    public override void startObject()
    {
        base.startObject();   
        stayDelay = 0.3f;
        order = ObjectOrder.wall;

        hp = 100;
        immortalTimeForHit = 0.5f;
        foundRange = 5;
        canFall = true;
    }


    public override bool canMoveOn(Vector2Int point)
    {
        bool tmpReturn = map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
        if( map.getMapObjects<MapObject>(point.x,point.y, x => x.isCollisiable) != null) 
        {
            if(map.getMapObjects<MapObject>(point.x, point.y, x => x is PushableObject) != null) {
                tmpReturn = (map.getMapObjects<Box>(point.x, point.y, x => x.objectName == "Box")[0].tryPush(movements.Peek().point));
                if (!tmpReturn)
                {
                    var tmpMov = movements.Peek();

                    var tmpInt = tmpMov.point.x;
                    tmpMov.point.x = tmpMov.point.y;
                    tmpMov.point.y = tmpInt;

                    addMovement(tmpMov);
                }
                return tmpReturn;
            }
        }

        tmpReturn = map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
        if (!tmpReturn)
        {
            var tmpMov = movements.Peek();

            var tmpInt = tmpMov.point.x;
            tmpMov.point.x = tmpMov.point.y;
            tmpMov.point.y = tmpInt;

            addMovement(tmpMov);
        }
        return tmpReturn;
    }


    override public void onStartWalk()
    {
        base.onStartWalk();
        
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        if (Mathf.Abs(target.position.x - position.x) >= Mathf.Abs(target.position.y - position.y))
        {
            if (target.position.x - position.x > 0)
                addMovement(new movement(new Vector2Int(1,0), true));
            else
                addMovement(new movement(new Vector2Int(-1,0), true));
        }
        else
        {
            if (target.position.y - position.y > 0)
                addMovement(new movement(new Vector2Int(0,1), true));
            else
                addMovement(new movement(new Vector2Int(0,-1), true));
        }
    }

    public override void updateObject()
    {
        base.updateObject();
    }

    public override void firstLook()
    {
        //Debug.Log("Ээ!");
        if (Mathf.Abs(target.position.x - position.x) >= Mathf.Abs(target.position.y - position.y))
        {
            if (target.position.x - position.x > 0)
                addMovement(new movement(new Vector2Int(1,0), true));
            else
                addMovement(new movement(new Vector2Int(-1,0), true));
        }
        else
        {
            if (target.position.y - position.y > 0)
                addMovement(new movement(new Vector2Int(0,1), true));
            else
                addMovement(new movement(new Vector2Int(0,-1), true));
        }

    }

    public UsualStalker(int x, int y): base(x,y) {
        
    }
}

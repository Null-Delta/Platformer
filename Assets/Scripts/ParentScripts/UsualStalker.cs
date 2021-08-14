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
                tmpReturn = (map.getMapObjects<Box>(point.x, point.y, x => x.objectName == "Box")[0].tryPush(movements[0].point));
                return tmpReturn;
            }
        }

        tmpReturn = map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
        return tmpReturn;
    }


    override public void updateObject()
    {
        base.updateObject();
    }


    public override void foundWay()
    {
        if (foundTarget)
            if (Mathf.Abs(target.position.x - position.x) + Mathf.Abs(target.position.y - position.y) > rangeOfAttack)
            {
                if (Mathf.Abs(target.position.x - position.x) >= Mathf.Abs(target.position.y - position.y))
                {
                    if (target.position.x - position.x > 0)
                        if (map.getMapObjects<MapObject>((int)position.x+1, (int)position.y, x => x.isCollisiable ) == null)
                            addMovement(new movement(new Vector2Int(1,0), true));
                        else
                        {
                            if (target.position.y - position.y > 0)
                                addMovement(new movement(new Vector2Int(0,1), true));
                            else if (target.position.y - position.y < 0)
                                addMovement(new movement(new Vector2Int(0,-1), true));
                        }
                    else
                        if (map.getMapObjects<MapObject>((int)position.x-1, (int)position.y, x => x.isCollisiable ) == null)
                            addMovement(new movement(new Vector2Int(-1,0), true));
                        else
                        {
                            if (target.position.y - position.y > 0)
                                addMovement(new movement(new Vector2Int(0,1), true));
                            else if (target.position.y - position.y < 0)
                                addMovement(new movement(new Vector2Int(0,-1), true));
                        }
                }
                else
                {
                    if (target.position.y - position.y > 0)
                        if (map.getMapObjects<MapObject>((int)position.x, (int)position.y+1, x => x.isCollisiable ) == null)
                            addMovement(new movement(new Vector2Int(0,1), true));
                        else
                        {
                            if (target.position.x - position.x > 0)
                                addMovement(new movement(new Vector2Int(1,0), true));
                            else if (target.position.x - position.x < 0)
                                addMovement(new movement(new Vector2Int(-1,0), true));
                        }
                    else
                        if (map.getMapObjects<MapObject>((int)position.x, (int)position.y-1, x => x.isCollisiable ) == null)
                            addMovement(new movement(new Vector2Int(0,-1), true));
                        else
                        {
                            if (target.position.x - position.x > 0)
                                addMovement(new movement(new Vector2Int(1,0), true));
                            else if (target.position.x - position.x < 0)
                                addMovement(new movement(new Vector2Int(-1,0), true));
                        }
                }
            }
            else
            {
                isAttack = true;
                startOfAttack();
            }
    }


    public override void firstLook()
    {
        foundWay();
    }



    public UsualStalker(int x, int y): base(x,y) {
        
    }
}

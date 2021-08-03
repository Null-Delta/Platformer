using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsualStalker : Seeker
{
    public override string objectName => "UsualStalker";
    
    public bool isAttack = false;
    public float rangeOfAttack = 1;

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


    override public void updateObject()
    {
        base.updateObject();
        if (movements.Count == 0)
            foundTarget = false;
    }


    override public void onStartWalk()
    {
        base.onStartWalk();
        
    }

    override public void onEndWalk() 
    {
        base.onEndWalk();
        if (foundTarget)
            if (Mathf.Abs(target.transform.position.x - position.x) + Mathf.Abs(target.transform.position.y - position.y) > rangeOfAttack)
            {
                stayDelay = 0.3f;
                if (Mathf.Abs(target.transform.position.x - position.x) >= Mathf.Abs(target.transform.position.y - position.y))
                {
                    if (target.transform.position.x - position.x > 0)
                        addMovement(new movement(new Vector2Int(1,0), true));
                    else
                        addMovement(new movement(new Vector2Int(-1,0), true));
                }
                else
                {
                    if (target.transform.position.y - position.y > 0)
                        addMovement(new movement(new Vector2Int(0,1), true));
                    else
                        addMovement(new movement(new Vector2Int(0,-1), true));
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
        if (Mathf.Abs(target.transform.position.x - position.x) + Mathf.Abs(target.transform.position.y - position.y) > rangeOfAttack)
            if (Mathf.Abs(target.transform.position.x - position.x) >= Mathf.Abs(target.transform.position.y - position.y))
            {
                if (target.transform.position.x - position.x > 0)
                    addMovement(new movement(new Vector2Int(1,0), true));
                else
                    addMovement(new movement(new Vector2Int(-1,0), true));
            }
            else
            {
                if (target.transform.position.y - position.y > 0)
                    addMovement(new movement(new Vector2Int(0,1), true));
                else
                    addMovement(new movement(new Vector2Int(0,-1), true));
            }
        else
        {
            isAttack = true;
            startOfAttack();
        }

    }

    public virtual void startOfAttack()
    {
        Debug.Log(1);
    }



    public UsualStalker(int x, int y): base(x,y) {
        
    }
}

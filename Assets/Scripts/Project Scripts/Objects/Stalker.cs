using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : WalkAndLive
{
    public override string objectName => "Stalker";
    Player target;
    bool foundTarget;

    override public void onStartWalk() {

        base.onStartWalk();
        Debug.Log(1);
    }

    public override void startObject()
    {
        base.startObject();   
        stayDelay = 0.3f;
        order = ObjectOrder.wall;

        hp = 100;
        immortalTimeForHit = 0.5f;
        canFall = true;
    }
    public override void updateObject()
    {
        
        if (target == null)
        {
            target = Camera.main.GetComponent<PlayerControl>().CurrentPlayer;
            foundTarget = false;
        }
        else if (!foundTarget)
        {
            RaycastHit2D lookRay = Physics2D.Raycast(position, (target.position - position).normalized, 5, 9);
            if (lookRay.collider !=null && lookRay.collider.gameObject.layer == 3)
            {
                //Debug.DrawRay(position, (target.position - position));
                // видит игрока
                foundTarget = true;
                addMovement(new movement(new Vector2Int(-1,0), true));
            }
        }
        base.updateObject();
    }

    public override bool canMoveOn(Vector2Int point)
    {
        if( map.getMapObjects<MapObject>(point.x,point.y, x => x.isCollisiable) != null) 
        {
            if(map.getMapObjects<MapObject>(point.x, point.y, x => x is PushableObject) != null) {
                return (map.getMapObjects<Box>(point.x, point.y, x => x.objectName == "Box")[0].tryPush(movements.Peek().point));
            }
        }


        return map.getMapObjects<MapObject>(point.x, point.y, x => x.isCollisiable ) == null;
    }

    override public void onEndWalk() {
        base.onEndWalk();;
    }

    override public void onDeath()
    {
    
    }


    public Stalker(int x, int y): base(x,y) {
        
    }
}

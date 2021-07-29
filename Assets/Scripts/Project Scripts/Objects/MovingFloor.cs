using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : LiveFloor
{
    Walker onMe = null;
    Vector2 onMeSpeedVector2;
    Vector3 onMeSpeedVector3;
    float enterExitTime = 0;
    public override string objectName => "MovingFloor";

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y)+3;

        isCollisiable = false;
    }

    public void addWalkerOn(Vector2 point, Walker obj)
    {
        onMe = obj;
        enterExitTime = onMe.animation_time;
        onMeSpeedVector2 = (position - point)/enterExitTime;
        onMeSpeedVector3 = onMeSpeedVector2;
    }
    public override bool readyCheck()
    {
        return true;
    }
    public override void onWalkStart()
    {
        if(map.getMapObjects<MapObject>((int)(position.x + linearMove.x),(int)(position.y + linearMove.y), x=> x is Floor || x is LiveFloor) != null)
        {
            linearMove.x = -linearMove.x;
            linearMove.y = -linearMove.y;
        }


    }
    public override void onWalkAnimation(float time)
    {
        
        if (onMe != null)
        {
            if (enterExitTime>0)
            {
                enterExitTime-=time;
                onMe.position += onMeSpeedVector2;
                onMe.gameObject.transform.position += onMeSpeedVector3;
                onMe.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(onMe.gameObject.transform.position.y - 2);
            }
            else
            {

            }
        }
    }
    public override void onWalkFinish()
    {
        
    }
    public override void onCollizion(MapObject obj, Collision2D collision)
    {
        
    }

    public MovingFloor(float x, float y):base(x,y) {
        position = new Vector2(x,y);
        taked_points = new List<Vector2>();
        taked_points.Add(new Vector2((int)x,(int)y));
        linearMove = new Vector2();

        linearMove.x = 1;
        move_delay = 0;
        animation_time = 0.4f;
    }
}

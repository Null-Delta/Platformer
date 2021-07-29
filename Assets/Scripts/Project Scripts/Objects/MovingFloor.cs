using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : LiveFloor
{
    public Walker onMe = null;
    Vector2 onMeSpeedVector;
    float enterExitTime = 0;
    bool objExit = false;
    public override string objectName => "MovingFloor";


    Vector2 tmpf;
    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y)-100;

        isCollisiable = false;
    }

    public void addWalkerOn(Vector2 point, Walker obj)
    {
        onMe = obj;
        enterExitTime = onMe.animation_time;
        onMeSpeedVector = (position + (linearMove*enterExitTime/animation_time) - point)/enterExitTime;
        onMe.useMovingVector = false;
        map.removeMapObject(onMe.taked_points[0], onMe);
        onMe.taked_points.Remove(onMe.taked_points[0]); 

        tmpf = point;
    }
    public override bool readyCheck()
    {
        return true;
    }
    public override void onWalkStart()
    {
        if(map.getMapObjects<MapObject>((int)(position.x + linearMove.x),(int)(position.y + linearMove.y), x=> x is Floor || x is LiveFloor || x is Wall) != null)
        {
            linearMove.x = -linearMove.x;
            linearMove.y = -linearMove.y;
        }

        if (onMe != null && !objExit && enterExitTime>0)
        {
            onMeSpeedVector = (position + (linearMove*(enterExitTime)/animation_time) - (onMe.position + onMe.linearMove*enterExitTime/onMe.animation_time) )/(enterExitTime);
            Debug.Log(enterExitTime);
        }

    }
    public override void onWalkAnimation(float time)
    {
        
        if (onMe != null)
        {
            if (enterExitTime>0)
            {
                enterExitTime-=time;
                onMe.position += onMeSpeedVector*time;
                onMe.gameObject.transform.position += new Vector3(onMeSpeedVector.x*time,onMeSpeedVector.y*time,0);
                onMe.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(onMe.gameObject.transform.position.y - 2);
                if (objExit &&  enterExitTime <=0)
                {
                    map.moveMapObject(onMe.moving_vector, onMe);
                    onMe.gameObject.transform.position =new Vector3(onMe.moving_vector.x, onMe.moving_vector.y, 0);
                    onMe = null;
                    objExit = false;
                }
            }
            else
            {
                if (!onMe.halfMove || onMe.fict_move)
                {
                    onMe.position = position;
                    onMe.gameObject.transform.position = gameObject.transform.position;
                    onMe.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(onMe.gameObject.transform.position.y - 2);
                }
                else 
                {
                    objExit = true;
                    enterExitTime = onMe.animation_time;
                    var tmpTrueVec = new Vector2(Mathf.RoundToInt(onMe.moving_vector.x), Mathf.RoundToInt(onMe.moving_vector.y));
                    onMeSpeedVector = (tmpTrueVec - onMe.moving_vector)/enterExitTime;
                    onMe.moving_vector = tmpTrueVec;
                    onMe.useMovingVector = true;
                }
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

        linearMove.x = Random.Range(-1,2);
        move_delay = 0;
        animation_time = 0.5f;
    }
}

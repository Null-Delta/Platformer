using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : StaticMapObject
{
    public Vector2 linearMove;
    float sum_time;
    public float move_delay;
    public float animation_time;
    bool in_animation = false;
    Vector2 moving_vector;
    bool is_ready = true;
    bool fict_move = false;
    public List<Vector2> taked_points;
    public override string objectName => "Walker";
    


    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y)+3;

        linearMove.x = Random.Range(-1,2);
            if (Mathf.Abs(linearMove.x) == 0)
                linearMove.y = Random.Range(0,2)*2-1;

        move_delay = Random.Range(0.1f,0.5f);
        animation_time = Random.Range(0.1f,0.5f);
    }

    public override StateEvent stateCheck(float time) 
    { 
        is_ready = readyCheck();

        if(sum_time + time >= move_delay- 0.0001f && sum_time < move_delay && is_ready)
        {
            return new StateEvent(move_delay - sum_time, this);
        }
        if(sum_time + time >= move_delay + animation_time- 0.0001f && sum_time < move_delay + animation_time && is_ready)
        {
            return new StateEvent(move_delay + animation_time - sum_time, this);
        }
        return null;
    }

    public virtual bool readyCheck()
    {
        return true;
    }

    public virtual void onWalkStart()
    {
        if(map.getMapObjects<StaticMapObject>((int)(position.x + linearMove.x),
             (int)(position.y + linearMove.y), x => x.isDecoration == false || x as Walker != null) != null)
        {
            linearMove.x = -linearMove.x;
            linearMove.y = -linearMove.y;
        }
    }

    public virtual void onWalkAnimation()
    {
    }

    public virtual void onWalkFinish()
    {
        
    }

    public override void updateObject(float time) {
        
        sum_time+=time; //важно
        
        if (sum_time >= move_delay- 0.0001f && !in_animation && is_ready)
        {
            sum_time =move_delay;
            in_animation = true;
            fict_move = false;
            
            
            onWalkStart();

            moving_vector =new Vector2(linearMove.x,linearMove.y);
            moving_vector +=position;

            if (moving_vector == position)
                fict_move = true;

            if (!fict_move) 
            {
                taked_points.Add(new Vector2((int)moving_vector.x,(int) moving_vector.y));
                map.setWalkerPoint(new Vector2((int)moving_vector.x,(int) moving_vector.y), this);
            }         
        }
        
        if (in_animation)
        {
            if(sum_time >= move_delay + animation_time- 0.0001f) {
                sum_time = 0f;
                in_animation = false;
                position = moving_vector;
                
                if (!fict_move)
                {
                    map.deleteWalkerPoint(taked_points[0], this);
                    taked_points.Remove(taked_points[0]);
                }

                onWalkFinish();
                
            } else {
                
                position += new Vector2(linearMove.x * (time / animation_time),linearMove.y * (time / animation_time));
            }
            onWalkAnimation();
            gameObject.transform.position = position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y - 1);
        }
        
        // if (in_animation && sum_time >= move_delay + animation_time)
        // {
        //     Debug.Log(sum_time + " " + (move_delay + animation_time));
            
            
        //     position = moving_vector;
        //     gameObject.transform.position = position;
        // }
        
    }

    public override bool isCollizion(MapObject obj)
    {
        return true;
    }

    public override void onCollizion(MapObject obj, Vector2 orientation)
    {
        if(obj is Wall) {
                linearMove.x = -linearMove.x;
                linearMove.y = -linearMove.y;
        } else if (obj is Bullet) {
            map.deleteObject(this);
            //animation_time +=0.1f;
        }
    }
    public Walker(float x, float y) {
        position = new Vector2(x,y);
        taked_points = new List<Vector2>();
        taked_points.Add(new Vector2((int)x,(int)y));
        linearMove = new Vector2();
    }
}

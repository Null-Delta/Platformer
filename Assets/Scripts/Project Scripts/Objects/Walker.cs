using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MovableMapObject
{
    float sum_time;
    public float move_delay;
    public float animation_time;
    bool in_animation = false;
    Vector2 moving_vector;
    bool is_ready = true;
    public List<Vector2> taked_points;
    public override string objectName => "Walker";
    

    public LinearMove linearMove {
        get {
            return move as LinearMove;
        }
    }

    public override void startObject()
    {
        base.startObject();
        gameObject.transform.position = position;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = -(int)(position.y)+3;
        move = new LinearMove(0,0,1);

        linearMove.dx = Random.Range(-1,2);
            if (Mathf.Abs(linearMove.dx) == 0)
                linearMove.dy = Random.Range(0,2)*2-1;

        radius = 0.4f;
        move_delay = Random.Range(0.1f,0.5f);
        animation_time = Random.Range(0.1f,0.5f);
    }

    public override StateEvent stateCheck(float time) 
    { 

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

    public virtual void onWalkStart(bool is_wall)
    {
        if(is_wall)
        {
            linearMove.dx = -linearMove.dx;
            linearMove.dy = -linearMove.dy;
        }
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
            
            
            onWalkStart(map.getMapObjects<StaticMapObject>((int)(position.x + linearMove.dx * linearMove.speed),
             (int)(position.y + linearMove.dy * linearMove.speed), x => x.isDecoration == false) != null ||
              map.checkWalkerPoint(new Vector2(linearMove.dx * linearMove.speed+position.x,linearMove.dy * linearMove.speed+position.y)));

            moving_vector =new Vector2(linearMove.dx * linearMove.speed,linearMove.dy * linearMove.speed);
            moving_vector +=position;

            taked_points.Add(new Vector2((int)moving_vector.x,(int) moving_vector.y));
            map.setWalkerPoint(new Vector2((int)moving_vector.x,(int) moving_vector.y), this);            
        }
        
        if (in_animation)
        {
            if(sum_time >= move_delay + animation_time- 0.0001f) {
                sum_time = 0f;
                in_animation = false;
                position = moving_vector;

                map.deleteWalkerPoint(taked_points[0]);
                taked_points.Remove(taked_points[0]);

                onWalkFinish();
                
            } else {
                position += new Vector2(linearMove.dx * linearMove.speed * (time / animation_time),linearMove.dy * linearMove.speed * (time / animation_time));
            }

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
                linearMove.dx = -linearMove.dx;
                linearMove.dy = -linearMove.dy;
        } else if (obj is Bullet) {

            animation_time +=0.1f;
        }
    }
    public Walker(float x, float y) {
        position = new Vector2(x,y);
        taked_points = new List<Vector2>();
        taked_points.Add(new Vector2((int)x,(int)y));
    }
}
